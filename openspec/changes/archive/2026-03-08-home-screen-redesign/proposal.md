## Why

The current home screen is functional but visually basic — a simple search bar, horizontal category list, and a flat listings feed. The UI needs a redesign to match a modern sports marketplace aesthetic (inspired by the "FIELD." app reference), with better visual hierarchy, product card styling, and quick-access header actions.

## What Changes

- **Header redesign**: Replace current layout with app title ("SportowyHub") + 3 icon buttons (theme toggle, favorites, shopping cart placeholder)
- **Search bar**: Restyle the fake search bar to match the reference design (rounded, with filter icon)
- **Category chips**: Add default condition filters (ALL, NEW, USED) before the sport-section chips in a horizontally scrollable chip bar; selected chip gets filled background
- **"Hot Picks" section**: New horizontal carousel of featured/recent listing cards with:
  - Condition badge (NEW/USED) as colored overlay label
  - Favorite heart toggle button
  - Placeholder product image (colored background with sport icon)
  - Brand name, product title, price, optional original price (strikethrough), star rating
- **"All Products" section**: Redesigned vertical grid (2 columns) replacing the current flat list, with:
  - Sort button in section header
  - Result count label ("Showing N results")
  - Same card style as Hot Picks but in grid layout
- **Product image placeholders**: Since backend doesn't serve images yet, generate colored placeholder backgrounds with category-appropriate sport icons
- **Shopping cart button**: Add to header (icon only, no action implemented — placeholder for future feature)
- **Light/dark theme support**: Both themes supported as shown in reference images (keep existing color system)

## Capabilities

### New Capabilities

- `home-screen-layout`: Top-level home screen structure — header with action buttons, search bar, category chips, Hot Picks carousel, All Products grid, and overall scroll behavior
- `listing-card-ui`: Reusable listing card component used in both Hot Picks and All Products — condition badge, placeholder image, favorite toggle, brand/title/price/rating display
- `product-image-placeholder`: Placeholder image generation strategy for listings without backend images — colored backgrounds with sport category icons

### Modified Capabilities

- `listings-feed`: Current feed becomes two sections (Hot Picks horizontal + All Products grid) instead of a single vertical list; pagination applies to All Products section
- `filter-chips-ui`: Add ALL/NEW/USED condition chips before sport-section chips; chip selection filters listings by condition
- `favorites-toggle`: Heart button now appears on listing cards (Hot Picks and All Products) in addition to existing detail page placement
- `home-create-listing-fab`: FAB must coexist with the new grid layout and scroll behavior

## Impact

- **Views/Home/HomePage.xaml**: Major rewrite — new layout structure, card templates, sections
- **ViewModels/HomeViewModel.cs**: Add Hot Picks collection, condition filter state, sort state
- **Models/Api/ListingSummary.cs**: May need condition/brand fields (check backend API response)
- **Services/Listings/IListingsService.cs**: May need filter parameters (condition, sort)
- **Controls/**: New reusable `ListingCard` control or `DataTemplate`
- **Resources/**: Placeholder images or sport icon SVGs for product cards
- **No backend changes required** — uses existing API with possible new query parameters
