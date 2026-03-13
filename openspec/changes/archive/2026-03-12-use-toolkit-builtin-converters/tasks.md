## Tasks

- [x] Identify all XAML files referencing `InvertBoolConverter`, `IsNotNullConverter`, and `IntToBoolConverter`
- [x] Replace custom converter registrations in `App.xaml` with CommunityToolkit.Maui equivalents (`InvertedBoolConverter`, `IsNotNullConverter`, `IntToBoolConverter`)
- [x] Update all XAML references to use the new converter keys (kept same x:Key names — no XAML changes needed)
- [x] Delete `InvertBoolConverter.cs`, `IsNotNullConverter.cs`, `IntToBoolConverter.cs`
- [x] Build and verify all pages using these converters render correctly
