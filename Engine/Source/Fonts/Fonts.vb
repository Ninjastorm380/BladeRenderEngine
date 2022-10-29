Imports System.Drawing
Imports System.Drawing.Text
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Threading

Public Module Fonts
    Private FontCollection As New PrivateFontCollection
    Private MonoRuntimeFontFixConfigPath as String = "/home/" + WhoAmI_LinuxCall()+"/blade.monofontsworkaround.cfg"
    Private LoadedFontFiles as new list(of string)
    Private AlreadyExistingFontFiles as new list(of string)
    Private UserFontCacheAlreadyExists as Boolean = False
    Friend WorkaroundState as integer = 0
    Friend WorkaroundFinished As Boolean = False
    Friend CustomFontsLoaded As Boolean = False
    
    Public Function LoadFont(Path as string) As FontFamily()
        Dim OldList as FontFamily() = FontCollection.Families
        if LoadedFontFiles.Contains(Path) = False
            Dim FontMemBuffer as Byte() = File.ReadAllBytes(Path)
            Dim FontMemHandle as GCHandle = GCHandle.Alloc(FontMemBuffer, GCHandleType.Pinned)
            FontCollection.AddMemoryFont(FontMemHandle.AddrOfPinnedObject(),FontMemBuffer.Length)
            FontMemHandle.Free()
            LoadedFontFiles.Add(Path)
        End If
        Dim NewList as FontFamily() = FontCollection.Families
        
        if NewList.Length > OldList.Length
            Dim Result(NewList.Length-OldList.Length-1) as FontFamily
            Dim ResultIndex as Integer = 0
            For each x in NewList
                If OldList.Contains(x) = False
                    Result(ResultIndex) = X
                    ResultIndex += 1
                End If
            Next
            CustomFontsLoaded = True
            Return Result
        Else
            CustomFontsLoaded = True
            Return NewList.ToArray()
        End If

    End Function
    
    Public Sub UnloadFont(FontFamilies as FontFamily())
        For each x in FontFamilies
            x.Dispose()
            x = Nothing
        Next
    End Sub
    
    Friend Sub CheckMonoFontsWorkaroundBegin
        WorkaroundFinished = False
        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) = True
            If File.Exists(MonoRuntimeFontFixConfigPath) = True
                LoadConfigFromDisk()
                WorkaroundState = 2
            Else
                if LoadedFontFiles.count>0
                    Dim UserFontCachePath as String = "/home/" + WhoAmI_LinuxCall() + "/.local/share/fonts"
                    UserFontCacheAlreadyExists = Directory.Exists(UserFontCachePath)
                
                    if UserFontCacheAlreadyExists = True
                        Dim PreexistingFontFiles as String() = Directory.GetFiles(UserFontCachePath)
                        For Each x in LoadedFontFiles
                            Dim DestinationPath as String = UserFontCachePath + "/" + Path.GetFileName(X)
                            if PreexistingFontFiles.Contains(DestinationPath) = True
                                AlreadyExistingFontFiles.Add(DestinationPath)
                            Else 
                                File.Copy(x,DestinationPath)
                            End If
                        Next
                        if LoadedFontFiles.Count = AlreadyExistingFontFiles.Count then workaroundstate = 3
                    Else
                        Directory.CreateDirectory(UserFontCachePath)
                        For Each x in LoadedFontFiles
                            Dim DestinationPath as String = UserFontCachePath + "/" + Path.GetFileName(X)
                            File.Copy(x,DestinationPath)
                        Next
                    End if
                    if WorkaroundState <> 3
                        UpdateFontCache_LinuxCall_Async()
                        SaveConfigToDisk()
                        WorkaroundState = 1
                    End If
                Else 
                    WorkaroundState = 3
                End If
            End If
        End If
    End Sub

    Friend Sub CheckMonoFontsWorkaroundEnd
        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) = True and File.Exists(MonoRuntimeFontFixConfigPath) = True and WorkaroundState = 2
            Dim UserFontCachePath as String = "/home/" + WhoAmI_LinuxCall() + "/.local/share/fonts"
            if UserFontCacheAlreadyExists = False
                Directory.Delete(UserFontCachePath,True)
            Else
                For each x in LoadedFontFiles
                    Dim SelectedFontFile as string = UserFontCachePath + "/" + Path.GetFileName(x)
                    If AlreadyExistingFontFiles.Contains(SelectedFontFile) = False Then File.Delete(SelectedFontFile)
                Next
            End If
            UpdateFontCache_LinuxCall_Async()
            File.Delete(MonoRuntimeFontFixConfigPath)
        End If
    End Sub
    
    Private Sub SaveConfigToDisk()
        Dim Writer as new StreamWriter(MonoRuntimeFontFixConfigPath)
        Writer.WriteLine(convert.ToString(UserFontCacheAlreadyExists))
        Writer.WriteLine(Convert.ToString(AlreadyExistingFontFiles.Count))
        for each x in AlreadyExistingFontFiles
            Writer.WriteLine(x)
        Next
        Writer.Flush()
        Writer.Close()
        Writer.Dispose()
    End Sub
    
    Private Sub LoadConfigFromDisk()
        Dim Reader as new StreamReader(MonoRuntimeFontFixConfigPath)
        UserFontCacheAlreadyExists = convert.ToBoolean(Reader.ReadLine())
        Dim ExistingCount as Int32 = convert.ToInt32(Reader.ReadLine())
        For x = 0 to ExistingCount - 1
           AlreadyExistingFontFiles.Add((reader.ReadLine()))
        Next
        Reader.Close()
        Reader.Dispose()
    End Sub
    
    Private Function WhoAmI_LinuxCall as String
        Dim Info as new ProcessStartInfo
        Info.FileName = "whoami"
        Info.UseShellExecute = False
        Info.CreateNoWindow = True
        Info.RedirectStandardOutput = True
            
        Dim CallProcess as Process = Process.Start(Info)
        Dim Result as String = CallProcess.StandardOutput.ReadLine()
        CallProcess.WaitForExit()
        return Result
    End Function
    
    Private Sub UpdateFontCache_LinuxCall()
        Dim Info as new ProcessStartInfo
        Info.FileName = "fc-cache"
        Info.Arguments = "-f -v"
        Info.UseShellExecute = False
        Info.CreateNoWindow = True
        Info.RedirectStandardOutput = True
            
        Dim CallProcess as Process = Process.Start(Info)
        CallProcess.WaitForExit()
        WorkaroundFinished = True
    End Sub
    
    Private sub UpdateFontCache_LinuxCall_Async()
      Dim AsyncThread As new Thread(Sub() UpdateFontCache_LinuxCall()) : AsyncThread.Start()
    End sub
End Module