using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SportowyHub.Models.Api;
using SportowyHub.Resources.Strings;
using SportowyHub.Services.ListingManagement;
using SportowyHub.Services.Media;
using SportowyHub.Services.Locale;
using SportowyHub.Services.Navigation;
using SportowyHub.Services.Sections;
using SportowyHub.Services.Toast;

namespace SportowyHub.ViewModels;

public partial class CreateEditListingViewModel(
    IListingManagementService listingManagementService,
    ISectionsService sectionsService,
    IMediaService mediaService,
    ILocaleService localeService,
    INavigationService nav,
    IToastService toastService) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsSaving { get; set; }

    [ObservableProperty]
    public partial bool IsEditMode { get; set; }

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PriceText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Currency { get; set; } = "PLN";

    [ObservableProperty]
    public partial string CityId { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string VoivodeshipId { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Section? SelectedSection { get; set; }

    [ObservableProperty]
    public partial Category? SelectedCategory { get; set; }

    public ObservableCollection<Section> Sections { get; } = [];
    public ObservableCollection<Category> Categories { get; } = [];
    public ObservableCollection<MediaItem> Photos { get; } = [];

    public string PageTitle => IsEditMode
        ? AppResources.EditListingTitle
        : AppResources.CreateListingTitle;

    private string? _listingId;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id) && id is string idStr && !string.IsNullOrWhiteSpace(idStr))
        {
            _listingId = idStr;
            IsEditMode = true;
        }

        OnPropertyChanged(nameof(PageTitle));
    }

    [RelayCommand]
    private async Task Appearing(CancellationToken ct)
    {
        if (Sections.Count == 0)
        {
            await LoadSections(ct);
        }

        if (IsEditMode && !string.IsNullOrEmpty(_listingId))
        {
            await LoadExistingListing(ct);
        }
    }

    [RelayCommand]
    private async Task LoadSections(CancellationToken ct)
    {
        var result = await sectionsService.GetSectionsAsync(ct: ct);
        if (result.IsSuccess)
        {
            Sections.Clear();
            foreach (var section in result.Data!.Sports)
            {
                Sections.Add(section);
            }
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    [RelayCommand]
    private async Task LoadCategories(CancellationToken ct)
    {
        if (SelectedSection is null)
        {
            return;
        }

        var result = await sectionsService.GetCategoriesAsync(SelectedSection.Id, ct: ct);
        if (result.IsSuccess)
        {
            Categories.Clear();
            foreach (var category in result.Data!.Categories)
            {
                Categories.Add(category);
            }
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }

    partial void OnSelectedSectionChanged(Section? value)
    {
        SelectedCategory = null;
        Categories.Clear();
        if (value is not null)
        {
            LoadCategoriesCommand.Execute(null);
        }
    }

    private async Task LoadExistingListing(CancellationToken ct)
    {
        IsLoading = true;

        var result = await listingManagementService.GetMyListingAsync(_listingId!, ct);
        if (result.IsSuccess)
        {
            Title = result.Data!.Title;
            PriceText = result.Data.Price?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
            Currency = result.Data.Currency ?? "PLN";
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task Save(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            await toastService.ShowError(AppResources.ListingTitleRequired);
            return;
        }

        if (SelectedCategory is null)
        {
            await toastService.ShowError(AppResources.ListingCategoryRequired);
            return;
        }

        IsSaving = true;

        try
        {
            decimal? price = decimal.TryParse(PriceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var p)
                ? p
                : null;

            int.TryParse(CityId, out var cityId);
            int.TryParse(VoivodeshipId, out var voivodeshipId);

            var localeInfo = await localeService.GetLocaleInfoAsync(ct);
            var contentLocale = localeInfo.Locale;

            if (IsEditMode && !string.IsNullOrEmpty(_listingId))
            {
                var request = new UpdateListingRequest(
                    SelectedCategory.Id,
                    Title,
                    Description,
                    price,
                    Currency,
                    cityId > 0 ? cityId : null,
                    voivodeshipId > 0 ? voivodeshipId : null,
                    null,
                    null,
                    null,
                    contentLocale);

                var result = await listingManagementService.UpdateListingAsync(_listingId, request, ct);
                if (!result.IsSuccess)
                {
                    await toastService.ShowError(result.ErrorMessage!);
                    return;
                }

                await toastService.ShowSuccess(AppResources.ListingEditSuccess);
            }
            else
            {
                var request = new CreateListingRequest(
                    SelectedCategory.Id,
                    Title,
                    Description,
                    price,
                    Currency,
                    cityId,
                    voivodeshipId,
                    null,
                    null,
                    null,
                    contentLocale);

                var result = await listingManagementService.CreateListingAsync(request, ct);
                if (!result.IsSuccess)
                {
                    await toastService.ShowError(result.ErrorMessage!);
                    return;
                }

                await toastService.ShowSuccess(AppResources.ListingCreateSuccess);
            }

            await nav.GoBackAsync();
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task AddPhoto(CancellationToken ct)
    {
        if (!IsEditMode || string.IsNullOrEmpty(_listingId))
        {
            await toastService.ShowError(AppResources.ListingPhotoSaveFirst);
            return;
        }

        try
        {
            var results = await MediaPicker.Default.PickPhotosAsync();
            foreach (var file in results)
            {
                await using var stream = await file.OpenReadAsync();
                var result = await mediaService.UploadAsync(_listingId, stream, file.FileName, ct: ct);
                if (result.IsSuccess)
                {
                    Photos.Add(result.Data!);
                }
                else
                {
                    await toastService.ShowError(result.ErrorMessage!);
                }
            }
        }
        catch (Exception ex)
        {
            await toastService.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private async Task DeletePhoto(MediaItem item, CancellationToken ct)
    {
        var result = await mediaService.DeleteAsync(item.Id, ct);
        if (result.IsSuccess)
        {
            Photos.Remove(item);
        }
        else
        {
            await toastService.ShowError(result.ErrorMessage!);
        }
    }
}
