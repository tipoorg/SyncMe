#Set your preferred major app version
$SyncMeMajorVersion = 1
#Set your preferred minor app version
$SyncMeMinorVersion = 1

$GitCurrentBranchName = git rev-parse --abbrev-ref HEAD
$GitBranchName = git show-ref --quiet refs/heads/master && echo master
$GitCommitCount = git rev-list --count --first-parent HEAD
echo "Commit count on current branch ($GitCurrentBranchName): $GitCommitCount"
$GitBranchCommitCount = git rev-list --count --first-parent "$GitBranchName.."
echo "Ahead of target branch ($GitBranchName) on $GitBranchCommitCount commits"
$GitCommintCountExceptCurrentBranch = $GitCommitCount - $GitBranchCommitCount
echo "Commit difference between current and target: $GitCommintCountExceptCurrentBranch"

$BuildNumber = "$GitCommintCountExceptCurrentBranch" + "$GitBranchCommitCount"
$Version = "$SyncMeMajorVersion.$SyncMeMinorVersion.$BuildNumber"
$CurrentFolderPath = Get-Location

$androidManifestPath = Join-Path $CurrentFolderPath 'src\SyncMe\SyncMe.Android\Properties\AndroidManifest.xml'
$infoPlistPath = Join-Path $CurrentFolderPath 'src\SyncMe\SyncMe.IOS\Info.plist'

function XmlPoke {
    param(
    [Parameter(Mandatory=$true)] [string] $FilePath,
    [Parameter(Mandatory=$true)] [string] $XPath,
    [Parameter(Mandatory=$true)] [string] $Value,
    [HashTable] $NamespaceUrisByPrefix
    )

    $document = [System.Xml.XmlDocument]::new()
    $document.PreserveWhitespace = $true
    $document.Load((Resolve-Path $FilePath))

    $namespaceManager = [System.Xml.XmlNamespaceManager]::new($document.NameTable)

    if ($null -ne $NamespaceUrisByPrefix) {
        foreach ($prefix in $NamespaceUrisByPrefix.Keys) {
            $namespaceManager.AddNamespace($prefix, $NamespaceUrisByPrefix[$prefix]);
        }
    }

    $document.SelectSingleNode($XPath, $namespaceManager).InnerText = $Value
    $document.Save((Resolve-Path $FilePath))
}

# Set Android app version
$Namespaces = @{ android="http://schemas.android.com/apk/res/android" }
XmlPoke $androidManifestPath 'manifest/@android:versionCode' $BuildNumber $Namespaces
XmlPoke $androidManifestPath 'manifest/@android:versionName' $Version $Namespaces

# Set iOS app version
XmlPoke $infoPlistPath '//dict/key[. = ''CFBundleVersion'']/following-sibling::string[1]' $BuildNumber 
XmlPoke $infoPlistPath '//dict/key[. = ''CFBundleShortVersionString'']/following-sibling::string[1]' $Version 