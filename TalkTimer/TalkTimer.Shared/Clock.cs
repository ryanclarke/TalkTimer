﻿using System;
namespace TalkTimer
{
    public class Clock
    {
        private const int SecondsPerMinute = 60;

        private double _seconds;
        private DateTime _target;

        public int Raw { get { return (int)_seconds; } }
        public string Minutes { get { return ToWholeMinutes(Raw).ToString("0"); } }
        public string Seconds { get { return WithWholeMinutesRemoved(Raw).ToString("00"); } }

        public Clock(int minutes, int seconds = 0)
        {
            Set(minutes, seconds);
        }

        public void Set(int minutes, int seconds = 0)
        {
            _seconds = ToRaw(minutes, seconds);
            _target = DateTime.Now.AddSeconds(_seconds);
        }

        public void UpdateClock()
        {
            _seconds = (_target - DateTime.Now).TotalSeconds;
        }

        public void Resume()
        {
            _target = DateTime.Now.AddSeconds(_seconds);
        }

        public bool IsAt(int minutes, int seconds = 0)
        {
            return Raw == ToRaw(minutes, seconds);
        }

        public bool JustPassed(int minutes, int seconds = 0)
        {
            return Raw == ToRaw(minutes, seconds) - 1;
        }

        internal bool IsInOvertime()
        {
            return Raw <= 0;
        }

        internal bool IsLastMinute()
        {
            return Raw < SecondsPerMinute && Raw > 0;
        }

        private static int ToRaw(int minutes, int seconds = 0)
        {
            return ToSeconds(minutes) + seconds;
        }

        private static int ToSeconds(int minutes)
        {
            return minutes * SecondsPerMinute;
        }

        private static int ToWholeMinutes(int seconds)
        {
            return seconds / SecondsPerMinute;
        }

        private static int WithWholeMinutesRemoved(int seconds)
        {
            return seconds % SecondsPerMinute;
        }

        internal void IncrementMinute()
        {
            _seconds = _seconds + SecondsPerMinute;
        }

        internal void DecrementMinute()
        {
            _seconds = _seconds - SecondsPerMinute;
        }
    }
}
