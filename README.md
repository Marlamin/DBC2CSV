# DBC2CSV
This project aims to be a stand-alone version of the WoW.tools DBC/DB2 -> CSV export functionality.

## Usage
### Definitions
While definitions are included at release time, these are likely to be outdated when exporting DB2s from a more recent version of WoW. Hopefully up-to-date definitions can be found on [here](https://github.com/wowdev/WoWDBDefs) (to download, click Code -> Download ZIP), to update definitions overwrite the `definitions` folder with the `definitions` folder from the downloaded version of the WoWDBDefs repo.

### Hotfixes
Hotfix files (DBCache.bin) can be supplied to the application as well, but please make sure these are from the same build of the game as the DB2s you are giving to the application, there might be crashes/malformed output otherwise.

### Converting 
Exported CSV files will be placed in the same directory as the DBC/DB2 file, but with a .csv extension instead.

#### Drag and drop (Windows example)  
Drop one or multiple .dbc or .db2 file(s), or a folder containing .dbc or .db2 files on top of DBC2CSV.exe to convert it. Dropping a hotfix file (DBCache.bin) on the executable together with other arguments will also apply the hotfixes from said hotfix file(s) to exported CSVs. 

#### Command line  
Supported arguments (in any order/combination/amount):
- A file ending in .dbc or .db2
- A file ending in .bin (see "Hotfixes" above)
- A directory with .dbc and/or .db2 and/or .bin files 
