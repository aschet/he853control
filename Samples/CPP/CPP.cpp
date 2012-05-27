#import "..\..\HE853.tlb"

int main(int argc, char* argv[])
{
	CoInitialize(NULL);
	
	HE853::IDevicePtr device(__uuidof(HE853::Device));
	if (device->Open())
	{
	  device->SwitchOn(1001);
	  device->Close();
	}

	device->Release();

	CoUninitialize();
	
	return 0;
}
