using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autopilot.Resources
{
	class Inputs
	{
		public static string[] keyboardInputs = 
		{
			"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
			"1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
			"kp_1", "kp_2", "kp_3", "kp_4", "kp_5", "kp_6", "kp_7", "kp_8", "kp_9", "kp_0",
			"~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "_", "+", "=",
			"[", "]", "{", "}", "\\", "|", ";", ":", "'", "\"", ",", ".", "/", "<", ">", "?",
			"tab", "space", "backspace", "ins", "del", "home", "end", "pgup", "pgdn", "enter",
			"kp_enter", "kp_+", "kp_-", "kp_*", "kp_/", "kp_.",
			"numlk", "capslk", "scrlk", "pause", "break", "prntscr", "sysrq", "esc",
			"up", "down", "left", "right",
			"f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12",
			"f13", "f14", "f15", "f16", "f17", "f18", "f19", "f20", "f21", "f22", "f23", "f24"
		};

		public static string[] keyboardModifierInputs =
		{
			"ctrl", "alt", "altgr", "shift", "win", "menu"
		};

		public static string[] mouseInputs =
		{
			"mouse1", "mouse2", "mouse3", "scrup", "scrdown", "scrleft", "scrright",
			"mouse4", "mouse5", "mouse6", "mouse7", "mouse8", "mouse9", "mouse10", "mouse11", "mouse12", "mouse13", "mouse14", "mouse15"
		};
	}
}
