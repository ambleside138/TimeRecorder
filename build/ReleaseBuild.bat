rmdir /s /q Release
mkdir Release

dotnet publish "../src/TimeRecorder.sln" -c Release --self-contained  -r win10-x64 -o "Release" > build.log