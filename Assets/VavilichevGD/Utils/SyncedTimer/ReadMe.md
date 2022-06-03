# Synced timer

Synced timer is a simple timer system allows you create timers just with next line of code:


```
var timer = new SyncedTimer(timerType);
```

*where **timerType** - is a type of timer. There are four types of timer now:

- UpdateTick,
- UpdateTickUnscaled,
- OneSecTick,
- OneSecTickUnscaled

Chose those that more suit you and do the things. It has two events: TimerValueChanged, that frequence depends on the type of the timer. And TimerFinished - it's clean.

### Methods:

- Start();
- Stop();
- Pause();
- Unpause();
- SetTime(float newRemainingSeconds);
