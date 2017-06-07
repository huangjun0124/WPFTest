using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Test
{
    class DisposeInterface
    {
        public static void Test()
        {
            using (SimpleClass c1 = new SimpleClass())
            {
                // do things
            }
            // above code is same as below effct
            SimpleClass c2 = new SimpleClass();
            try
            {
                // do things
            }
            catch (Exception ex)
            {
            }
            finally
            {
                c2.Dispose();
            }
        }
    }

    class AnotherResource
    {
        public string ResourceName;

        public void Dispose()
        {
            // do nothing here
        }
    }

    class SimpleClass : IDisposable
    {
        /*对象被调用过Dispose方法，并不表示该对象已经被置为null，且被垃圾回收机制回收过内存，已经彻底不存在了。
         * 事实上，对象的引用可能还在，但是对象被Dispose过，说明对象的正常状态已经不在了，此时如果调用对象
         * 公开的方法，应该会为调用者抛出一个ObjectDisposedException
         */
        public void SimplePublicMethod()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SimpleClass","Simple Class is disposed");
            }
        }

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private AnotherResource managedResource = new AnotherResource();

        private bool disposed = false;
        public void Dispose()
        {
            // 必须为 true
            Dispose(true);
            // 通知垃圾回收机制不再调用终结器(析构器)
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            Dispose();
        }

        // 必须的，为了防止程序员忘了显示调用Dispose方法
        ~SimpleClass()
        {
            Dispose(false);
        }

        // 非密封类修饰用 protected virtural
        // 密封类修饰用private
        // 如果不为类型提供这个受保护的虚方法，很有可能让开发者设计子类的时候忽略掉父类的清理工作。所以，基于继承体系的原因
        // 要为类型的Dispose模式提供一个受保护的虚方法
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                // 清理托管资源
                if (managedResource != null)
                {
                    managedResource.Dispose();
                    managedResource = null;
                }
            }
            // 清理非托管资源
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
            // 让类型知道自己已经被释放
            disposed = true;
        }
    }

    class DerivedSampleClass : SimpleClass
    {
        private IntPtr derivedNativeResource = Marshal.AllocHGlobal(100);
        private bool derivedDisposed = false;

        // 非密封类修饰用 protected virtural
        // 密封类修饰用private
        protected virtual void Dispose(bool disposing)
        {
            if (derivedDisposed)
            {
                return;
            }
            if (derivedNativeResource != null)
            {
                Marshal.FreeHGlobal(derivedNativeResource);
                derivedNativeResource = IntPtr.Zero;
            }
            base.Dispose(disposing);
            derivedDisposed = true;
        }
    }
}
