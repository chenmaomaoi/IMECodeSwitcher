using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common;
using Core.Enums;
using Core.Models;

namespace Core.Event;

public delegate void OnConfigSavedEventHandler(AppConfigModel config);

public delegate void OnProgramIMECodeChangedEventHandler(string programName, IMECode imeCode);

public delegate void OnForegroundProgramChangedEventHandler(Win32Window window, string programName);