set UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2022.3.26f1\Editor\Unity.exe"
set PROJECT_PATH="%WORKSPACE%\JenkinsTest"
set BUNDLE_TOOL_PATH="%WORKSPACE%\jenkins_scripts\Tools\bundletool-all-1.18.0.jar"
set APK_NAME=TestFreeStyle_29_.apk
set EXPORT_APK_PATH="%WORKSPACE%\Export\TestFreeStyle_29_.apk"
set BUILD_AAB=true
set GOOGLE_PLAY_AAB_PATH="%WORKSPACE%\Export\googlePlay.aab"
set OUT_PUT_APKS_PATH_1="%WORKSPACE%\Export\output_1.apks"
set OUT_PUT_APKS_PATH_2="%WORKSPACE%\Export\output_2.apks"
set SAMSUNG_S9_PATH="%WORKSPACE%\jenkins_scripts\Tools\Samsung_S9.json"

echo "this is android_distribute.bat"
echo WORKSPACE=%WORKSPACE%
echo UNITY_PATH=%UNITY_PATH%
echo PROJECT_PATH=%PROJECT_PATH%
echo BUNDLE_TOOL_PATH=%BUNDLE_TOOL_PATH%
echo APK_NAME=%APK_NAME%
echo EXPORT_APK_PATH=%EXPORT_APK_PATH%
echo BUILD_AAB=%BUILD_AAB%
echo GOOGLE_PLAY_AAB_PATH=%GOOGLE_PLAY_AAB_PATH%
echo OUT_PUT_APKS_PATH_1=%OUT_PUT_APKS_PATH_1%
echo OUT_PUT_APKS_PATH_2=%OUT_PUT_APKS_PATH_2%

%UNITY_PATH% -projectPath %PROJECT_PATH% -buildTarget android -executeMethod GameEditor.APKBuild.ExportAPKManual -logfile - -batchMode -quit -GMMode
echo "Export apk and aab success"

echo "this is android_distribute.bat end"
