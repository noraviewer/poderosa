Option Explicit

' ��1������DLL�t�@�C����T������f�B���N�g���p�X�ł��B
Dim dirPath
dirPath = "."
Dim oParam
Set oParam = WScript.Arguments
If oParam.Count > 0 Then
  dirPath = oParam(0)
End If

' ��������DLL�̃t�@�C�������󔒕�����؂�ŗ񋓂��ĕW���o�͂ɏ����o���܂��B
Dim fso, folder, file, subFolder
Set fso = WScript.CreateObject("Scripting.FileSystemObject")
Set folder = fso.GetFolder(dirPath)
WScript.StdOut.WriteLine filelist(folder)

' �w�肳�ꂽ�f�B���N�g������DLL�t�@�C�������󔒋�؂�ŗ񋓂��ĕԂ��܂��B
' ���DLL�t�@�C�������݂��Ȃ��ꍇ�͢Nothing��������Ԃ��܂��B
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
