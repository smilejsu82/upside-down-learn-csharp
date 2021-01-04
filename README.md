# upside-down-learn-csharp (거꾸로 배우는 C#)

> 그동안 책을 펴고 1장부터 공부 하셨다면 이제는 거꾸로 마지막장부터 공부할 차례입니다.

## 목차

### 30. Process & Thread

Process : 실행파일이 실행되어 메모리에 적재된 인스턴스  
Thread : 프로세스내에서 실행되는 흐름의 단위를 말한다

각 프로세스는 주 스레드라고 하는 단일 스레드로 시작한다.  
모든 스레드에서 추가 스레드를 만들수 있다  
프로세스 내의 모든 스레드는 해당 프로세스의 주소 공간을 공유 한다

1시간짜리 영화를 비디오플레이어를 통해 재생했다  
단일 쓰레드로 만들었다면 프로세스를 중지 하기 전까지 재생되는 영화를 정지할수 없을것이다  
사용자 입력에 대한 쓰레드를 추가 한다면 영화를 보면서 사용자 입력을 받을수 있을것이다

프로세스끼리 데이터를 교환 하려면 소켓이나 공유메모리(Inter Process Communication)를 이용해야 한다  
프로세스 간 통신(Inter-Process Communication, IPC)이란 프로세스들 사이에 서로 데이터를 주고받는 행위 또는 그에 대한 방법이나 경로를 뜻한다.

간단한 IPC 통신 예제를 살펴 본다

```C#
using System;
using System.IO;
using System.IO.Pipes;

namespace ipc_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("press any key to connect server");
            Console.ReadKey();
            string message = null;
            Console.WriteLine("wait for connection...");
            using (NamedPipeServerStream stream = new NamedPipeServerStream("testpipe", PipeDirection.In))
            {
                stream.WaitForConnection();
                StreamReader reader = new StreamReader(stream);
                message = reader.ReadToEnd();
            }
            Console.WriteLine("message recived : " + message);
        }
    }
}
```

```C#
using System;
using System.IO;
using System.IO.Pipes;

namespace ipc_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("press any key to start server");
            Console.ReadKey();

            using (NamedPipeClientStream stream = new NamedPipeClientStream(".", "testpipe", PipeDirection.Out, PipeOptions.None))
            {
                Console.WriteLine(stream);
                if (!stream.IsConnected)
                {
                    stream.Connect(1000);
                    Console.WriteLine("server connected!");
                }
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("Hello World!");
                writer.Flush();
            }
        }
    }
}

```

1. Thread 인스턴스 생성 (매개변수로 쓰레드가 실행할 메서드)
2. Thread.Start() 메서드 호출
3. Thread.Join() 메서드 호출 하여 쓰레드가 끝날때까지 대기

https://docs.microsoft.com/ko-kr/dotnet/api/system.threading.thread?view=net-5.0

```C#
using System;
using System.Threading;

namespace thread
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(SayHello));
            t1.Start();
            t1.Join();
        }

        static void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

실제 쓰레드가 메모리에 적재 되는 시점은 t1.Start() 메서드를 호출 하는 시점이다
