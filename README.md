# DBC2CSV
This project aims to be a stand-alone version of the WoW.tools DBC/DB2 -> CSV export functionality.

## Usage
### Definitions
While definitions are included at release time, these are likely to be outdated when exporting DB2s from a more recent version of WoW. Hopefully up-to-date definitions can be found on [here](https://github.com/wowdev/WoWDBDefs), to update definitions overwrite the `definitions` folder with the `definitions` folder from the downloaded version of the WoWDBDefs repo.

### Converting 
Exported CSV files will be placed in the same directory as the DBC/DB2 file, but with a .csv extension instead.

#### Drag and drop (Windows)  
Drop a .dbc or .db2 file, or a folder containing .dbc or .db2 files on top of DBC2CSV.exe to convert it.  

#### Command line  
First argument should be a .dbc or .db2 or a folder containing .dbc or .db2 files.


