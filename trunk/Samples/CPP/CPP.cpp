#import "..\..\HE853.tlb"

using namespace HE853;

int main(int argc, char* argv[])
{
	CoInitialize(NULL);
	
	IDevicePtr device(__uuidof(Device));

	device->Open();
	device->SwitchOn(1001, CommandStyle_Comprehensive);
	device->SwitchOff(1001, CommandStyle_Comprehensive);
	device->Close();

	device->Release();

	CoUninitialize();
	
	return 0;
}
