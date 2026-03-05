using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WinUITestProject.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;//プロパティが変更されたときのイベント

        //プロパティ変更を通知するためのメソッド
        //nameは変更されたプロパティ名
        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string? name = null)
        {
            //今の値と新しい値が同じなら何もしない
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
}


//んーーー！！！わかんねぇや！！！！！
/*
 * 全体として何やってるかはわかるんだけど、
 * 各プロパティが何を意味するのかわからん
 */