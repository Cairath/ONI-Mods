[CmdletBinding()]
Param (
    [switch] $NoDel
)

.\lib\7za.exe a ..\Mods\^CompletePack.zip "..\Mods (unzipped)\*"

if (!$NoDel) {
    Remove-Item -Path  "..\Mods (unzipped)" -Recurse
}