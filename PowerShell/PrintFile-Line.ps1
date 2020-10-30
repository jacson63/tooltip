#ファイルの指定行を表示する
#
#listFile:filename,lineのcsv
#
Param(
    [parameter(mandatory=$true)]
    [String]
    $listFile
)

Get-Content $listFile -Encoding UTF8 | ConvertFrom-Csv -Header "fileName", "line" | ForEach-Object{   
    $_.fileName + ":::" + (Get-Content $_.fileName -Encoding UTF8)[$_.line]
}
