# Property-Converter
A small Tool to convert C# AutoProperties to Mvvm Styled Properties.
You just enter your usual AutoProperties (styled like: public int index { get; set; }. This is basically what you get through using the VS snippet prop),
click the button and the output will be a Mvvm styled Property like this:

private int _index;
public int index
{
get { return _index; }
set { SetProperty(ref _index, value, () => index); }
}

This style shown here, is the style the library DevExpress.Mvvm (available for free through NuGet) is using. Support for more libraries is planned, the frame for implementation is already set.
MVVM Light (with "RaisePropertyChanged") is supported as well.
You can save the output directly to your clipboard or save it to a cs file. 
It is also supported to enter a bunch of properties (separated by line) and convert them all in one go.


![preview](https://github.com/al-develop/Property-Converter/blob/master/propconverter.png)



# For Developers - What can I do to contribute?
It would be good to have more options on the Mvvm Styles
Ideas would be to support:

- Prism
- Catel.Mvvm
- Caliburn.Micro
- ... and  everythign else what YOU might need to use (maybe your own implementations of Mvvm as well?)

Current Implementations are DevExpress.Mvvm and MVVM Light (both available through NuGet)

You can either do it your own way, or look up how I converted it for the DevExpress or Mvvm Light style (string manipulation) and just change the key points to your own style.
Also, the code structure is pure evil. If you feel like it, you can clean up and refactor the code as you want (I used just code-behind and everythign is in there currently. Ugly, dirty but I needed the tool done quickly - sorry 'bout that)
Before contributing your own awesome implementations, you might want to clean it all up, as long as it's not too late for refactoring. If I will add more converters, I guess I will be up to that bothersome task.

Used Libraries:
- Ookii.Dialogs.Wpf (NuGet)
