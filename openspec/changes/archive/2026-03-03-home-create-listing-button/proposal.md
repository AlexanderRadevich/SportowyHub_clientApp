## Why

The create listing button currently exists only on the My Listings page (accessed via Profile → My Listings). Users on the home feed — the first screen they see — have no way to start creating a listing without navigating through Profile first. Adding a FAB to the home page reduces friction and makes listing creation a primary action.

## What Changes

- Add a floating action button (FAB) to the home page that navigates to the create/edit listing page
- Gate the FAB visibility based on user authentication state — only show for logged-in users
- When a TL0 user taps the FAB, show a message directing them to verify their phone (the backend rejects listing creation for TL0)

## Capabilities

### New Capabilities
- `home-create-listing-fab`: FAB on the home page for creating a listing, with auth and trust-level gating

### Modified Capabilities
- `listings-feed`: Home page layout changes to include the FAB overlay

## Impact

- `Views/Home/HomePage.xaml` — add FAB button overlaid on the listings feed
- `ViewModels/HomeViewModel.cs` — add navigation command and trust-level check logic
- `Services/Auth/IAuthService.cs` — may need to expose current user trust level for the check
