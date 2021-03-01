#
# Author: Eastrall
# Description: Tests the Redstone solution
#

function Test-Project {
    param (
        $ProjectName
    )

    dotnet test tests/$ProjectName/$ProjectName.csproj `
        /p:CollectCoverage=true `
        /p:Exclude="[xunit*]*" `
        /p:CoverletOutputFormat=opencover `
        /p:CoverletOutput="../TestResults/$ProjectName.xml" `
        /maxcpucount:1
}

function main {
    Test-Project -ProjectName 'Redstone.Common.Tests'
    Test-Project -ProjectName 'Redstone.NBT.Tests'
    Test-Project -ProjectName 'Redstone.Protocol.Tests'
    Test-Project -ProjectName 'Redstone.Server.Tests'
}

main