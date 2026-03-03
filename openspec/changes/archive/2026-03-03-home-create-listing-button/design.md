## Context

The home page (`HomePage.xaml`) shows a listings feed with search bar, pull-to-refresh, and infinite scroll. There is no way to create a listing from this screen. The only create listing FAB exists on `MyListingsPage`, reachable via Profile → My Listings. The `HomeViewModel` currently depends on `IListingsService`, `INavigationService`, and `IToastService` — it has no auth awareness.

`IAuthService.GetCurrentUserAsync()` returns `UserInfo(Id, Email, TrustLevel)` where `TrustLevel` is a string like `"TL0"`, `"TL1"`, etc. `IAuthService.IsLoggedInAsync()` checks auth state. Both are available for injection.

The create/edit listing page is already registered as route `"create-edit-listing"` in `AppShell.xaml.cs`.

## Goals / Non-Goals

**Goals:**
- Add a FAB to the home page that navigates to the create listing page
- Show the FAB only for authenticated users
- Show a phone verification prompt for TL0 users who tap the FAB

**Non-Goals:**
- Changing the My Listings page FAB (it stays as-is)
- Adding trust-level gating to the My Listings page FAB
- Implementing phone verification flow (already exists)

## Decisions

**Decision 1: Reuse the same FAB pattern from MyListingsPage**

The existing FAB on `MyListingsPage` is a 56x56 circular `Button` with `+` text, positioned bottom-right with 16dp margin. Reuse the same visual pattern for consistency.

**Decision 2: Inject `IAuthService` into `HomeViewModel`**

Add `IAuthService` to the `HomeViewModel` primary constructor. On page appearing, check `IsLoggedInAsync()` and set an `IsLoggedIn` observable property to control FAB visibility. This avoids blocking the feed load.

**Decision 3: Trust-level check on tap, not on visibility**

Show the FAB to all logged-in users regardless of trust level. When tapped:
- TL1+ → navigate to `create-edit-listing`
- TL0 → show a toast explaining phone verification is required

This is simpler than hiding/showing based on trust level and gives TL0 users a clear call-to-action rather than a mysteriously missing button.

Alternative considered: hide FAB for TL0 — rejected because users wouldn't know the feature exists.

**Decision 4: Check auth state in `OnAppearing` alongside existing load**

The `HomePage.xaml.cs` code-behind already calls `LoadListingsCommand` in `OnAppearing`. Add a `CheckAuthCommand` that runs in parallel to set `IsLoggedIn`. This keeps the feed load unblocked.

## Risks / Trade-offs

- [Risk] Auth state changes while the home page is visible (e.g., token expires) → Mitigation: Re-check in `OnAppearing` which fires on every navigation back to the page.
- [Risk] FAB overlaps the last listing card on small screens → Mitigation: Same 16dp margin pattern already works on MyListingsPage.
