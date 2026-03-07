namespace SportowyHub.Models.Api;

public record GeographyAutocompleteItem(
    string Type,
    int? VoivodeshipId = null,
    int? CityId = null,
    string? Name = null,
    string? Label = null);
