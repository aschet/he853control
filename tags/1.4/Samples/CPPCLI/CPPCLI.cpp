using namespace System;
using namespace System::IO;

int main(array<String ^> ^args)
{
	HE853::IDevice^ device = gcnew HE853::Device();
	try
	{
		device->Open();
		device->SwitchOn(1001, false);
		device->SwitchOff(1001, false);
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
