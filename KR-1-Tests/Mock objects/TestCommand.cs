using KR_1.Commands;

namespace KR_1_Tests.Mock_objects;

class TestCommand : ICommand
{
    public bool Executed { get; private set; }
    public void Execute() => Executed = true;
}