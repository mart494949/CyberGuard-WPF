# CyberGuard: Educational Cybersecurity Simulator 🛡️

![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?style=flat&logo=dotnet)
![WPF](https://img.shields.io/badge/WPF-Windows_Desktop-blue)
![Architecture](https://img.shields.io/badge/Architecture-MVVM-success)
![License](https://img.shields.io/badge/License-MIT-green)

> **Elevator Pitch:** CyberGuard is an interactive desktop application designed to bridge the gap between gamification and cybersecurity education. It simulates network packet filtering and phishing analysis to teach users about digital threats in a safe, engaging environment.


## 🚀 Features

* **Firewall Arcade Module:** Real-time packet filtering simulation. Users act as a firewall, neutralizing malicious incoming traffic (viruses) while allowing safe data packets to pass.
* **Phishing Analysis Module:** An interactive scenario-based module teaching users how to identify social engineering attempts and malicious emails.
* **Dynamic Educational Feedback:** The system implements "punishment by education." Upon system compromise (Game Over), users are presented with randomized, real-world cybersecurity tips.
* **Data Persistence:** User progress and module configurations are serialized and saved locally using JSON.

## 🛠️ Tech Stack & Architecture

The application is built with a strong emphasis on clean code and separation of concerns, strictly following the **MVVM (Model-View-ViewModel)** architectural pattern.

* **Language/Framework:** C# / .NET 6.0 / Windows Presentation Foundation (WPF)
* **Architecture:** MVVM (Custom implementation with `ObservableObject` and `RelayCommand`)
* **Data Handling:** `Newtonsoft.Json` for serialization/deserialization.
* **UI/UX:** Native XAML with custom Converters (`IValueConverter`) and dynamic Data Binding.

## ⚙️ Setup & Installation

1. Clone the repository:
   ```bash
   git clone [https://github.com/](https://github.com/)[YourUsername]/CyberGuard-WPF.git
