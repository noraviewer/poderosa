Option Explicit

' 第1引数はDLLファイルを探索するディレクトリパスです。
Dim dirPath
dirPath = "."
Dim oParam
Set oParam = WScript.Arguments
If oParam.Count > 0 Then
  dirPath = oParam(0)
End If

' 見つかったDLLのファイル名を空白文字区切りで列挙して標準出力に書き出します。
Dim fso, folder, file, subFolder
Set fso = WScript.CreateObject("Scripting.FileSystemObject")
Set folder = fso.GetFolder(dirPath)
WScript.StdOut.WriteLine filelist(folder)

' 指定されたディレクトリ内のDLLファイル名を空白区切りで列挙して返します。
' 一つもDLLファイルが存在しない場合は｢Nothing｣文字列を返します。
Function filelist(folder)
  Dim fso
  Dim line
  Dim fileExt
  Set fso = WScript.CreateObject("Scripting.FileSystemObject")
  For Each file In folder.Files
    fileExt = fso.GetExtensionName(file)
    If LCase(fileExt) = "dll" Then
      line = line & " " & file.Name
    End If
  Next
  If line = vbEmpty Then
    line = "Nothing"
  End If
  filelist = line  
End Function
