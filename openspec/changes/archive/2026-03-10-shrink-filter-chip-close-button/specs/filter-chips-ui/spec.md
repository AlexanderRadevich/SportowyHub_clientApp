## ADDED Requirements

### Requirement: Search page filter chip close button size
The close (remove) button on active filter chips in the search page SHALL be 10×10 logical pixels. The spacing between the chip label and the close button SHALL be 4 logical pixels.

#### Scenario: Close button renders at reduced size
- **WHEN** a filter chip is displayed on the search page
- **THEN** the close button icon SHALL render at 10×10 logical pixels

#### Scenario: Label-to-button spacing
- **WHEN** a filter chip is displayed on the search page
- **THEN** the gap between the label text and the close button SHALL be 4 logical pixels
