#pragma once

struct Checkpoint {
	int            index     = -1;
	float          timeStamp = -1.0f;
	const wchar_t* name      = nullptr;
};
