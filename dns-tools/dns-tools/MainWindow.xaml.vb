Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Windows.Shell
Imports Microsoft.Win32

Class MainWindow

    Private Sub openGithub()
        Process.Start("https://github.com/houshuu/FzDNSLite")
    End Sub

    Private Sub openBlog()
        Process.Start("https://www.fang2hou.com/")
    End Sub

    Private Sub Unblock_OnClick_1()
        Dim correctNet = GetGateway()
        If checkIPv6() Then
            MsgBox("初次使用需要修改IPv6参数，重启后解锁功能生效。")
        End If
        Process.Start("cmd", "ipconfig /flushdns & /c netsh interface ip set dns name=""" + correctNet + """ static 158.69.209.100 primary validate=no & netsh interface ipv4 add dnsserver """ + correctNet + """ 8.8.8.8 index=2 & ipconfig /flushdns")
    End Sub

    Private Sub Unblock_OnClick_2()
        Dim correctNet = GetGateway()
        If checkIPv6() Then
            MsgBox("初次使用需要修改IPv6参数，重启后解锁功能生效。")
        End If
        Process.Start("cmd", "ipconfig /flushdns & /c netsh interface ip set dns name=""" + correctNet + """ static 45.32.72.192 primary validate=no & netsh interface ipv4 add dnsserver """ + correctNet + """ 8.8.8.8 index=2 & ipconfig /flushdns")
    End Sub

    Private Sub Unblock_OnClick_3()
        Dim correctNet = GetGateway()
        If checkIPv6() Then
            MsgBox("初次使用需要修改IPv6参数，重启后解锁功能生效。")
        End If
        Process.Start("cmd", "ipconfig /flushdns & /c netsh interface ip set dns name=""" + correctNet + """ static 45.63.69.42 primary validate=no & netsh interface ipv4 add dnsserver """ + correctNet + """ 8.8.8.8 index=2 & ipconfig /flushdns")
    End Sub

    Private Sub Reset_OnClick()
        Dim correctNet = GetGateway()
        Process.Start("cmd", "/c ipconfig /flushdns & netsh interface ip set dnsservers name=""" + correctNet + """ source=dhcp & ipconfig /flushdns")
    End Sub

    Private Sub ResetIPv6_OnClick()
        Dim correctNet = GetGateway()
        If recoverIPv6() Then
            MsgBox("成功还原IPv6参数，重启后彻底还原成原样。")
        End If
        Process.Start("cmd", "/c ipconfig /flushdns & netsh interface ip set dnsservers name=""" + correctNet + """ source=dhcp & ipconfig /flushdns")
    End Sub

    Private Function checkIPv6() As Boolean
        Dim needReboot As Boolean
        Dim ipv6StatusKeyword As String

        needReboot = False
        ipv6StatusKeyword = getIPv6Status()

        If ipv6StatusKeyword = "255" Then
            needReboot = False
        Else
            If disableIPv6() Then
                needReboot = True
            End If
        End If

        Return needReboot
    End Function

    Private Function disableIPv6() As Boolean
        Dim disableCheck As Boolean

        disableCheck = False

        My.Computer.Registry.SetValue _
        ("HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters\", "DisabledComponents", "255", RegistryValueKind.DWord)

        If getIPv6Status() = "255" Then
            disableCheck = True
        End If

        Return disableCheck
    End Function

    Private Function recoverIPv6() As Boolean
        Dim disableCheck As Boolean

        disableCheck = False

        My.Computer.Registry.SetValue _
        ("HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters\", "DisabledComponents", "0", RegistryValueKind.DWord)

        If getIPv6Status() = "0" Then
            disableCheck = True
        End If

        Return disableCheck
    End Function

    Private Function getIPv6Status() As String
        Dim readValue As String
        readValue = My.Computer.Registry.GetValue _
        ("HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters\", "DisabledComponents", Nothing)
        Return readValue
    End Function

    Private Function GetLocalIP() As String
        Dim strLocalIP = ""
        Dim strPcName = Dns.GetHostName()
        Dim ipEntry = Dns.GetHostEntry(strPcName)
        Dim ip As IPAddress = New IPAddress(New Byte() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1})
        Dim ip2 As IPAddress = New IPAddress(New Byte() {127, 0, 0, 1})
        For Each IPadd In ipEntry.AddressList
            If IPadd.Equals(ip) OrElse IPadd.Equals(ip2) Then
                Continue For
            End If
            strLocalIP = IPadd.ToString()
            Exit For
        Next
        Return strLocalIP
    End Function

    Private Function GetGateway() As String
        Dim correctNet = ""
        Dim strGateway = ""
        Dim nics = NetworkInterface.GetAllNetworkInterfaces()
        For Each netWork In nics
            Dim ip = netWork.GetIPProperties()
            Dim gateways = ip.GatewayAddresses
            correctNet = netWork.Name
            For Each gateWay In gateways
                If (IsPingIP(gateWay.Address.ToString())) Then
                    strGateway = gateWay.Address.ToString()
                    Exit For
                End If
            Next
            If (strGateway.Length > 0) Then Exit For
        Next
        Return correctNet
    End Function

    Public Shared Function IsPingIP(strIP As String) As Boolean
        Try
            Dim ping = New Ping()
            Dim reply = ping.Send(strIP, 1000)
            Return True
        Catch
        End Try
        Return False
    End Function

    Public Function GetDnsIPv4Addresses() As IPAddress
        For Each item In GetDnsAddresses()
            If (item.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork) Then
                Return item
            End If
        Next
        Return Nothing
    End Function

    Public Function GetDnsAddresses() As IPAddressCollection

        Dim adapters = NetworkInterface.GetAllNetworkInterfaces()
        For Each adapter In adapters
            Dim adapterProperties = adapter.GetIPProperties()
            Return adapterProperties.DnsAddresses
        Next

        Return Nothing
    End Function
End Class