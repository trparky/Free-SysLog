# Free SysLog

**A lightweight, no-frills syslog server for Windows.**

Free SysLog is designed for people who need a simple way to collect and view syslog messages from routers, switches, and other network devices—without setting up complex systems like Splunk or Graylog.

## Why I built this

I couldn’t find a good, simple syslog server for Windows that just worked.

So I made one.

## Features

- Real-time syslog message viewing
- Supports standard syslog formats (RFC 5424)
- Powerful regex-based filtering
- Custom alerting with extracted data
- Lightweight — no database, no server setup
- Built specifically for Windows

## Screenshots
![image](https://github.com/trparky/Free-SysLog/assets/32105035/8b0783ef-c5b1-4450-870f-0af30c0ee23a)

## Getting Started

1. Download the latest release
2. Run the executable
3. Point your device’s syslog output to your PC’s IP (port 514)
4. Logs appear instantly

## Windows SmartScreen Warning

Because this is a free and independently developed tool, it is not code-signed.

- Click **More Info**
- Click **Run Anyway**

This is expected and safe.

## Security & Transparency

This project is fully open source.  
The included binary is compiled from the exact source code in this repository—nothing hidden, nothing extra.

You can verify the download using the provided SHA256 hash on the release page.
