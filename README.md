# DBC2CSV
This project is a stand-alone version of the WoW.tools DB2 -> CSV export functionality.

## Requirements
### .NET 7.0
The .NET 7.0 runtime is required to run this application, it can be downloaded [here](https://dotnet.microsoft.com/en-us/download).
### Definitions
While definitions are included in releases, these are likely to be outdated when exporting DB2s from a more recent version of WoW. Hopefully up-to-date definitions can be found on [here](https://github.com/wowdev/WoWDBDefs) (to download, click Code -> Download ZIP), to update definitions overwrite the `definitions` folder with the `definitions` folder from the downloaded version of the WoWDBDefs repo.

## Download 
The latest version can be downloaded [here](https://github.com/Marlamin/DBC2CSV/releases).

## Supported files
### DBC/DB2s
This tool does not download or extract DB2 files for you. This needs to be done through a different application such as [WoW.tools](https://wow.tools/files) (while available), [CASCExplorer](https://github.com/WoW-Tools/CASCExplorer/releases) or [wow.export](https://www.kruithne.net/wow.export/) (DB2s aren't listed in wow.export by default, for that go to the menu on the top right and click "Browse Raw Client files"). 

_**Only files versioned WDB5+ (~Legion) and up will work.**_

### Hotfixes (DBCache.bin)
Hotfix files can be optionally supplied to the application as well, but please make sure these are from the same (or a compatible) build of the game as the DB2s you are giving to the application, there might be crashes/malformed output otherwise. DBCache.bin can be found in your WoW directory (e.g. `_retail_/Cache/ADB/enUS`).


## Usage
Exported CSV files will be placed in the same directory as the DBC/DB2 file, but with a .csv extension instead.

#### Drag and drop 
Drop one or multiple .dbc or .db2 file(s), or a folder containing .db2 files on top of the main binary (e.g. DBC2CSV.exe on Windows) to convert. Dropping a hotfix file (DBCache.bin) or a folder of hotfix files on the executable together with other arguments will also apply the hotfixes from said hotfix file(s) to exported CSVs. 

#### Command line  
Supported arguments (in any order/combination/amount):
- A file ending in .db2
- A file ending in .bin (see "Hotfixes" above)
- A directory with .db2 and/or .bin files 
