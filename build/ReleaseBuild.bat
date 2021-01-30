rmdir /s /q Release
mkdir Release

dotnet publish "../src/TimeRecorder/TimeRecorder.csproj" -c Release -r win10-x64 -p:PublishSingleFile=true --self-contained true -o "Release" > build.log

