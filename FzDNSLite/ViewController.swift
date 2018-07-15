//
//  ViewController.swift
//  FzDNSLite
//
//  Created by Fang Zhou on 2017/03/16.
//  Copyright © 2017 Fang2hou. All rights reserved.
//

import Cocoa
import Foundation

class ViewController: NSViewController {
    
    func runCommand(launchPath: String, arguments: [String]){
        let task = Process()
        task.launchPath = launchPath
        task.arguments = arguments
        task.launch()
    }
    
    func setDNSServer(address: String, displayText: String) {
        
        var adapter: String = "Wi-Fi"
        
        if adapterName.stringValue != "" {
            adapter = adapterName.stringValue
        }
        
        // Set DNSServer as [UY DNS, Google DNS]
        runCommand(launchPath: "/usr/bin/sudo", arguments: ["networksetup","-setdnsservers", adapter, address,"8.8.8.8"])
        
        // Disable IPv6
        runCommand(launchPath: "/usr/bin/sudo", arguments: ["networksetup","-setv6off",adapter])
        

        // Clear DNS Cache
        runCommand(launchPath: "/usr/bin/sudo", arguments: ["killall", "-HUP", "mDNSResponder"])
        
        // Display the success message
        displaySuccess.stringValue = adapter + ": " + displayText + " 执行成功"
    }
    
    @IBOutlet weak var adapterName: NSTextField!
    @IBOutlet weak var displaySuccess: NSTextField!
    
    @IBAction func openBlog(_ sender: Any) {
        if let url = URL(string: "https://www.fang2hou.com"), NSWorkspace.shared().open(url) {
            print("blog was successfully opened")
        }
    }
    @IBAction func openGithub(_ sender: Any) {
        if let url = URL(string: "https://github.com/houshuu/FzDNSTool-Lite-macOS"), NSWorkspace.shared().open(url) {
            print("github was successfully opened")
        }
    }

    @IBAction func unblockWithServer1(_ sender: NSButton) {
        setDNSServer(address: "158.69.209.100", displayText: sender.title)
    }
    
    @IBAction func unblockWithServer2(_ sender: NSButton) {
        setDNSServer(address: "45.32.72.192", displayText: sender.title)
    }

    @IBAction func unblockWithServer3(_ sender: NSButton) {
        setDNSServer(address: "45.63.69.42", displayText: sender.title)
    }

    @IBAction func oneKeyReset(_ sender: NSButton) {
        
        var adapter: String = "Wi-Fi"
        
        if adapterName.stringValue != "" {
            adapter = adapterName.stringValue
        }
        
        // Set DNS Server as default
        runCommand(launchPath: "/usr/bin/sudo", arguments: ["networksetup","-setdnsservers",adapter, "Empty"])
        
        // Re-enable IPv6
        runCommand(launchPath: "/usr/bin/sudo", arguments: ["networksetup","-setv6automatic",adapter])
        
        // Clear DNS Cache
        runCommand(launchPath: "/usr/bin/sudo", arguments: ["killall", "-HUP", "mDNSResponder"])
        
        // Display the success message
        displaySuccess.stringValue = adapter + ": " + sender.title + " 执行成功"
    }
    
    @IBOutlet weak var displayLogo: NSImageView!
}
