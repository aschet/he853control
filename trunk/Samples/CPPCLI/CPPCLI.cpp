#using "..\..\HE853.dll"

int main(array<System::String ^> ^args)
{
	HE853::IDevice^ device = gcnew HE853::Device();
	if (device->Open())
	{
		device->On(1001);
		device->Off(1001);
		device->Close();
	}

	return 0;
}
