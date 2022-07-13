using System;
public interface IUIInterface
{
    void MenuOn(Action beforeComplition, Action afterComplition);
    void MenuOff();
}
