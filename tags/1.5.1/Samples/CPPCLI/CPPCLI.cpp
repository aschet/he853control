using namespace System;
using namespace System::IO;
using namespace HE853;

int main(array<String ^> ^args)
{
	IDevice^ device = gcnew Device();
	try
	{
		device->Open();
		device->SwitchOn(1001, CommandStyle::Comprehensive);
		device->SwitchOff(1001, CommandStyle::Comprehensive);
		device->Close();
	}
	catch (FileNotFoundException^ exception)
	{
		Console::WriteLine(exception->Message);
	}
	catch (IOException^ exception)
	{
		Console::WriteLine(exception->Message);
		device->Close();
	}

	return 0;
}
