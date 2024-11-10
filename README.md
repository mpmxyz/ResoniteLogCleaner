# Resonite Log Cleaner
Removes personal information from Resonite logs

## About
This tool uses a few hard-coded patterns to extract information that may be personal:
- Windows user directory
- Machine ID
- Resonite user names
- Resonite user IDs
- Resonite world names
- Steam IDs

All found data will be searched and replaced within the the whole log file.

_Note_: There is no guarantee that this tool removes all personal data. Additionally it is possible to corrupt the log file. (i.e. if a user has a simple username like "of")

Take time to review the results before submitting the log to [Resonite-Issues](https://github.com/Yellow-Dog-Man/Resonite-Issues)!

The program will output the cleaned log file via stdout.
Information about replacements will be output via stderr.

## Installation
Unzip the archive into a directory of your choice.

I recommend Resonite's log directory or at least a location nearby so that it makes executing the program easier.
(disadvantage of log directory: you may accidentally delete the tool if you decide to "delete all logs")

## Usage (PowerShell)
`path-to/ResoniteLogCleaner.exe "original logfile.log" > "cleaned logfile.log"`
