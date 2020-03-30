#include "framework.h"
#include "Project1.h"
#include <vector>

#define MAX_LOADSTRING 100
#define ID_FIRSTCHILD 200

struct GameMode
{
	int width;
	int amount;
	int margin = 5;
};

GameMode Small{ 80,8 };
GameMode Medium{ 70, 10 };
GameMode Big{ 60,12 };
GameMode active = Small;

HINSTANCE hInst;
WCHAR szTitle[MAX_LOADSTRING];
WCHAR szWindowClass[MAX_LOADSTRING];
WCHAR szGemClass[] = L"GEMCLASS";

WCHAR szTopClass[] = L"TOPCLASS";
LRESULT CALLBACK	TopProc(HWND, UINT, WPARAM, LPARAM);
ATOM				MyRegisterClassTop(HINSTANCE hInstance);

WCHAR szPartClass[] = L"PARTCLASS";
LRESULT CALLBACK	PartProc(HWND, UINT, WPARAM, LPARAM);
ATOM				MyRegisterClassPart(HINSTANCE hInstance);
//Specific or general window functions
ATOM                MyRegisterClass(HINSTANCE hInstance);
ATOM                RegisterGem(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    GemProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
//functions for board operatons
VOID                CreateBoard(GameMode);
VOID                InvalidateBoard();
VOID                ColorBoard();
//functions for gems handling
VOID                TrackMouse(HWND);
int                 IndexGem(HWND);
HBRUSH              GemColor(HWND);
bool				IsBlocked(HWND);
//color associated global functions
HBRUSH				GetRandomColor();
//windows associated global variables
HWND hwnd, top;
HWND selected = NULL;
HWND* gems;
bool* blocked;
//colors associated global variables
HBRUSH* colors;
HBRUSH BlueBrush = CreateSolidBrush(RGB(0, 0, 255));
HBRUSH RedBrush = CreateSolidBrush(RGB(255, 0, 0));
HBRUSH GreenBrush = CreateSolidBrush(RGB(0, 255, 0));
HBRUSH YellowBrush = CreateSolidBrush(RGB(255, 255, 0));
HBRUSH FuchsiaBrush = CreateSolidBrush(RGB(255, 0, 255));
HBRUSH CyanBrush = CreateSolidBrush(RGB(0, 255, 234));
HBRUSH DefaultBrush = CreateSolidBrush(RGB(50, 50, 50));
HBRUSH KillingBrush = CreateHatchBrush(HS_CROSS, RGB(0, 0, 0));

//GAME LOGIC FUNCTIONS AND VARIABLES                          
std::vector<HWND> Marked;
HWND ToBeDeleted;
bool			GameInit = false;
bool			CheckBoard();
bool			CheckLine(int i);
bool			CheckColumn(int i);
void			Delete();
int* gamed;
bool black;

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPWSTR    lpCmdLine,
	_In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadStringW(hInstance, IDC_PROJECT1, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);
	RegisterGem(hInstance);
	MyRegisterClassTop(hInstance);
	MyRegisterClassPart(hInstance);

	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_PROJECT1));

	MSG msg;

	while (GetMessage(&msg, nullptr, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int)msg.wParam;
}


ATOM MyRegisterClassPart(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = PartProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PROJECT1));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = CreateSolidBrush(RGB(0,0,0));
	wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_PROJECT1);
	wcex.lpszClassName = szPartClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}

ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PROJECT1));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_PROJECT1);
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}

ATOM RegisterGem(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = GemProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PROJECT1));
	wcex.hCursor = LoadCursor(nullptr, IDC_HAND);
	wcex.hbrBackground = DefaultBrush;
	wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_PROJECT1);
	wcex.lpszClassName = szGemClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}

ATOM MyRegisterClassTop(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = TopProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PROJECT1));
	wcex.hCursor = LoadCursor(nullptr, IDC_HAND);
	wcex.hbrBackground = NULL;
	wcex.lpszMenuName = NULL;
	wcex.lpszClassName = szTopClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}



BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	//some vars init
	hInst = hInstance;
	colors = new HBRUSH[active.amount * active.amount];
	blocked = new bool[active.amount * active.amount];
	for (int i = 0; i < active.amount * active.amount; i++)
	{
		colors[i] = DefaultBrush;
	}
	//setup main and top windows
	

	

	hwnd = CreateWindowEx(WS_EX_CONTROLPARENT, szWindowClass, szTitle, WS_OVERLAPPEDWINDOW | WS_CLIPCHILDREN | WS_CLIPSIBLINGS ,
		0, 0, 0, 0, top, nullptr, hInstance, nullptr);
	top = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TRANSPARENT, szTopClass, L"TOP", WS_POPUP | WS_VISIBLE | WS_CHILD | WS_CLIPSIBLINGS, 0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN),
		hwnd, nullptr, hInstance, nullptr);
	SetWindowLong(top, GWL_EXSTYLE, GetWindowLong(top, GWL_EXSTYLE) | WS_EX_LAYERED);
	SetLayeredWindowAttributes(top, RGB(255, 255, 255), 0, LWA_COLORKEY);
	::SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SIZEBOX);
	CheckMenuItem(GetMenu(hwnd), ID_BOARDSIZE_SMALL, MF_CHECKED);
	CreateBoard(Small);
	if (!hwnd)
	{
		return FALSE;
	}

	ShowWindow(hwnd, nCmdShow);
	UpdateWindow(hwnd);
	

	LPCWSTR message = L"My Overlayed Window";
		RECT rect;
		HDC wdc = GetWindowDC(top);
		GetWindowRect(top, &rect);
		SetTextColor(wdc, RGB(255,0,0));
		
		SetBkMode(wdc, RGB(0,0,0));
		rect.left = GetSystemMetrics(SM_CXSCREEN)/2;
		rect.top = GetSystemMetrics(SM_CYSCREEN) / 4;
		DrawText(wdc, message, -1, &rect, DT_SINGLELINE);
		DeleteDC(wdc);

	ShowWindow(top, nCmdShow);
	UpdateWindow(top);
	return TRUE;
}
//window processes

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	static UINT checked = ID_BOARDSIZE_SMALL;
	switch (message)
	{
	case WM_COMMAND:
	{
		int wmId = LOWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case ID_BOARDSIZE_SMALL:
		{
			GameInit = false;
			InvalidateRect(hwnd, NULL, TRUE);
			UpdateWindow(hwnd);
			CheckMenuItem(GetMenu(hWnd), checked, MF_UNCHECKED);
			CheckMenuItem(GetMenu(hWnd), ID_BOARDSIZE_SMALL, MF_CHECKED);
			InvalidateBoard();
			CreateBoard(Small);
		}
		break;
		case ID_BOARDSIZE_MEDIUM:
		{
			GameInit = false;
			InvalidateRect(hwnd, NULL, TRUE);
			UpdateWindow(hwnd);
			CheckMenuItem(GetMenu(hWnd), checked, MF_UNCHECKED);
			CheckMenuItem(GetMenu(hWnd), ID_BOARDSIZE_MEDIUM, MF_CHECKED);
			InvalidateBoard();
			CreateBoard(Medium);
		}
		break;
		case ID_BOARDSIZE_BIG:
		{
			GameInit = false;
			InvalidateRect(hwnd, NULL, TRUE);
			UpdateWindow(hwnd);
			CheckMenuItem(GetMenu(hWnd), checked, MF_UNCHECKED);
			CheckMenuItem(GetMenu(hWnd), ID_BOARDSIZE_BIG, MF_CHECKED);
			InvalidateBoard();
			CreateBoard(Big);

		}
		break;
		case ID_GAME_NEWGAME:
		{
			GameInit = true;
			black = true;
			ColorBoard();
			InvalidateRect(hwnd, NULL, TRUE);
			UpdateWindow(hwnd);
			Sleep(3000);
			//analyze board
			CheckBoard();
			Delete();
			black = false;
			InvalidateRect(hwnd, NULL, TRUE);
			UpdateWindow(hwnd);
		}
		break;
		case SHOW_MENU:
		{
			RECT rc;
			GetWindowRect(hWnd, &rc);
			HMENU hmenu = GetSystemMenu(hWnd, FALSE);
			TrackPopupMenu(hmenu, 0, rc.left, rc.top, 0, hWnd, NULL);
		}
		break;
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	break;
	case WM_PAINT:
	{
		HDC hdc;
		HDC hdcMem;
		HBITMAP hbmMem;
		HANDLE hOld;
		RECT rect;
		RECT rc;
		PAINTSTRUCT ps;
		GetWindowRect(hwnd, &rect);
		GetWindowRect(hwnd, &rc);
		hdc = BeginPaint(hwnd, &ps);
		hdcMem = CreateCompatibleDC(hdc);
		hbmMem = CreateCompatibleBitmap(hdc, rc.right - rc.left, rc.right - rc.left);
		hOld = SelectObject(hdcMem, hbmMem);
		rc.top = 0;
		rc.left = 0;
		rc.right = rect.right - rect.left;
		rc.bottom = rect.top - rect.bottom;
		GetClientRect(hwnd, &rc);
		if (black)
		{
			FillRect(hdcMem, &rc, CreateSolidBrush(RGB(100, 100, 100)));
		}
		else {
			FillRect(hdcMem, &rc, CreateSolidBrush(RGB(255, 255, 255)));
		}
		BitBlt(hdc, 0, 0, rect.right - rect.left, rect.right - rect.left, hdcMem, 0, 0, SRCCOPY);
		SelectObject(hdcMem, hOld);
		DeleteObject(hbmMem);
		DeleteDC(hdcMem);
		EndPaint(hwnd, &ps);
	}
	break;
	case WM_GETMINMAXINFO:
	{
		MINMAXINFO* minMaxInfo = (MINMAXINFO*)lParam;
		int size = active.amount * (2 * active.margin + active.width);
		minMaxInfo->ptMaxSize.x = minMaxInfo->ptMaxTrackSize.x = size + 15;
		minMaxInfo->ptMaxSize.x = minMaxInfo->ptMinTrackSize.x = size + 15;
		minMaxInfo->ptMaxSize.y = minMaxInfo->ptMaxTrackSize.y = size + 60;
		minMaxInfo->ptMaxSize.y = minMaxInfo->ptMinTrackSize.y = size + 60;
	}
	break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

///////////////////////////////////////////////////////////////////
///////// WOHOOOO !!! I DID DOUBLE BUFFERING /////////////////////
/////////////////////////////////////////////////////////////////

LRESULT CALLBACK GemProc(HWND gem, UINT message, WPARAM wParam, LPARAM lParam)
{
	POINT pt;
	RECT rc;
	HBRUSH brush = GemColor(gem);
	BOOL Tracking = FALSE;
	HDC hdcMem;
	HBITMAP hbmMem;
	HANDLE hOld;
	PAINTSTRUCT ps;
	HDC hdc;
	switch (message)
	{
		break;
	case WM_LBUTTONDOWN:
	{
		if (GameInit)
		{
			HWND tmp = selected;
			if (selected != gem)
			{
				if (selected == NULL)
				{
					selected = gem;
					InvalidateRect(gem, NULL, TRUE);
					UpdateWindow(gem);
				}
				else
				{
					HBRUSH color_sel, tmp_b;
					color_sel = GemColor(selected);
					tmp_b = GemColor(gem);
					HWND topN, bottomN, leftN, rightN;
					int idx = IndexGem(selected);
					try
					{
						topN = gems[idx - active.amount];
					}
					catch (...) {
						topN = NULL;
					}
					try
					{
						bottomN = gems[idx + active.amount];
					}
					catch (...)
					{
						bottomN = NULL;
					}
					if (idx % active.amount == 0)
					{
						leftN = NULL;
					}
					else
					{
						leftN = gems[idx - 1];
					}
					if (idx % active.amount == (active.amount - 1))
					{
						rightN = NULL;
					}
					else
					{
						rightN = gems[idx + 1];
					}
					if (gem == rightN || gem == leftN || gem == bottomN || gem == topN)
					{
						bool check;
						colors[IndexGem(selected)] = tmp_b;
						colors[IndexGem(gem)] = color_sel;
						check = CheckBoard();
						if (check)
						{
							selected = NULL;
							InvalidateRect(tmp, NULL, TRUE);
							UpdateWindow(tmp);
							InvalidateRect(gem, NULL, TRUE);
							UpdateWindow(gem);

							Delete();
						}
						else
						{
							selected = NULL;
							colors[IndexGem(tmp)] = color_sel;
							colors[IndexGem(gem)] = tmp_b;
							InvalidateRect(tmp, NULL, TRUE);
							UpdateWindow(tmp);
							InvalidateRect(gem, NULL, TRUE);
							UpdateWindow(gem);
						}
					}
					selected = NULL;
					InvalidateRect(tmp, NULL, TRUE);
					UpdateWindow(tmp);

				}
			}
			else
			{
				selected = NULL;
				InvalidateRect(tmp, NULL, TRUE);
				UpdateWindow(tmp);
			}
		}
	}
	case WM_MOUSEMOVE:
	{
		if (!Tracking)
		{
			TrackMouse(gem);
			Tracking = TRUE;
		}
	}
	case WM_MOUSEHOVER:
	{
		if (!IsBlocked(gem))
		{
			GetWindowRect(gem, &rc);
			MapWindowPoints(HWND_DESKTOP, hwnd, (LPPOINT)&rc, 2);
			MoveWindow(gem, rc.left - 4, rc.top - 4, active.width + 4 * 2, active.width + 4 * 2, TRUE);
			blocked[IndexGem(gem)] = true;
			break;
		}
		KillTimer(gem, 7);
		break;
	}
	case WM_MOUSELEAVE:
	{
		Tracking = FALSE;
		SetTimer(gem, 7, 50, NULL);
		break;
	}
	case WM_TIMER:
	{
		switch (wParam)
		{
		case 7:
		{
			RECT rect;
			GetWindowRect(gem, &rect);
			MapWindowPoints(HWND_DESKTOP, hwnd, (LPPOINT)&rect, 2);
			if ((rect.right - rect.left) != active.width)
			{
				MoveWindow(gem, rect.left + 1, rect.top + 1, rect.right - rect.left - 2, rect.right - rect.left - 2, TRUE);
			}
			else {
				blocked[IndexGem(gem)] = 0;
				KillTimer(gem, 7);
			}
		}
		break;
		}
	}
	break;
	//DO NOT TOUCH THIS DOUBLE BUFFERING!!!!
	case WM_PAINT:
	{
		RECT rect;
		GetWindowRect(gem, &rect);
		GetWindowRect(gem, &rc);
		hdc = BeginPaint(gem, &ps);
		hdcMem = CreateCompatibleDC(hdc);
		hbmMem = CreateCompatibleBitmap(hdc, rc.right - rc.left, rc.right - rc.left);
		hOld = SelectObject(hdcMem, hbmMem);
		rc.top = 0;
		rc.left = 0;
		rc.right = rect.right - rect.left;
		rc.bottom = rect.top - rect.bottom;
		GetClientRect(gem, &rc);
		if (ToBeDeleted != gem)
		{
			FillRect(hdcMem, &rc, GemColor(gem));
			if (gem == selected)
			{
				rc.left = rc.left;
				rc.right = rc.right;
				rc.top = rc.top;
				rc.bottom = rc.bottom;
				FrameRect(hdcMem, &rc, CreateSolidBrush(RGB(0, 0, 0)));
				for (int i = 3; i > 0; i--)
				{
					rc.left = rc.left + 1;
					rc.right = rc.right - 1;
					rc.top = rc.top + 1;
					rc.bottom = rc.bottom - 1;
					FrameRect(hdcMem, &rc, CreateSolidBrush(RGB(0, 0, 0)));
				}
			}
		}
		else
		{
			FillRect(hdcMem, &rc, KillingBrush);
		}
		BitBlt(hdc, 0, 0, rect.right - rect.left, rect.right - rect.left, hdcMem, 0, 0, SRCCOPY);
		SelectObject(hdcMem, hOld);
		DeleteObject(hbmMem);
		DeleteDC(hdcMem);
		EndPaint(gem, &ps);
		return 0;
	}
	break;
	case WM_ERASEBKGND:
		return 1;
	default:
		return DefWindowProc(gem, message, wParam, lParam);
	}
	return 0;
}
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}

//board operations
VOID CreateBoard(GameMode gm)
{
	//get menu height : https://social.msdn.microsoft.com/Forums/en-US/4d96870d-6f3e-4ec9-a69d-fdde4b811f58/how-to-get-height-of-menu-bar-in-mfc?forum=vcmfcatl
	int cy_border = GetSystemMetrics(SM_CYFRAME);
	int cy_caption = GetSystemMetrics(SM_CYCAPTION);
	RECT window_rect;
	GetWindowRect(hwnd, &window_rect);
	POINT client_top_left = { 0, 0 };
	ClientToScreen(hwnd, &client_top_left);
	int menu_height = client_top_left.y - window_rect.top - cy_caption - cy_border;
	//init board values
	active = gm;
	colors = new HBRUSH[active.amount * active.amount];
	gems = new HWND[active.amount * active.amount];
	blocked = new bool[active.amount * active.amount];
	gamed = new int[active.amount * active.amount];
	//Move window to the center
	int width = (gm.margin + gm.width + gm.margin) * gm.amount;
	int xPos = GetSystemMetrics(SM_CXSCREEN) / 2 - width / 2;
	int yPos = GetSystemMetrics(SM_CYSCREEN) / 2 - width / 2;
	MoveWindow(hwnd, xPos, yPos - menu_height, width + 15, width + 2 * menu_height + active.margin * 2, TRUE);
	//Create children grid
	for (int i = 0; i < active.amount; i++)
	{
		for (int j = 0; j < active.amount; j++)
		{
			gems[i * active.amount + j] = CreateWindowEx(0, szGemClass, L"GEM",
				WS_CHILD | WS_VISIBLE,
				(gm.margin * 2 + gm.width) * j + gm.margin,
				(gm.margin * 2 + gm.width) * i + gm.margin,
				gm.width, gm.width,
				hwnd, (HMENU)(int)(ID_FIRSTCHILD + gm.amount * i + j), NULL, NULL);
			colors[i * active.amount + j] = DefaultBrush;
			blocked[i * active.amount + j] = false;
			gamed[i * active.amount + j] = 0;
		}
		UpdateWindow(hwnd);
	}

}
VOID InvalidateBoard() {
	for (int i = 0; i < active.amount * active.amount; i++)
	{
		DestroyWindow(gems[i]);
	}
	delete[] gems;
	delete[] colors;
	delete[] blocked;
	delete[] gamed;
}
VOID ColorBoard()
{
	for (int i = 0; i < active.amount * active.amount; i++)
	{
		colors[i] = GetRandomColor();
		InvalidateRect(gems[i], NULL, TRUE);
		UpdateWindow(gems[i]);
		Sleep(15);
	}
}

//gem handling functions
VOID TrackMouse(HWND gem)
{
	TRACKMOUSEEVENT track;
	track.cbSize = sizeof(TRACKMOUSEEVENT);
	track.hwndTrack = gem;
	track.dwFlags = TME_HOVER | TME_LEAVE;
	track.dwHoverTime = 1;
	TrackMouseEvent(&track);
}
int IndexGem(HWND gem)
{
	int i;
	for (i = 0; i < active.amount * active.amount; i++)
	{
		if (gems[i] == gem) return i;
	}
	return -100;
}
HBRUSH GemColor(HWND gem)
{
	return colors[IndexGem(gem)];
}
bool IsBlocked(HWND gem)
{
	return blocked[IndexGem(gem)];
}

//color functions
HBRUSH GetRandomColor()
{
	HBRUSH brush = NULL;
	int color = rand() % 6 + 1;
	switch (color)
	{
	case 1:
		brush = RedBrush;
		break;
	case 2:
		brush = BlueBrush;
		break;
	case 3:
		brush = GreenBrush;
		break;
	case 4:
		brush = CyanBrush;
		break;
	case 5:
		brush = FuchsiaBrush;
		break;
	case 6:
		brush = YellowBrush;
		break;
	}
	return brush;
}

//GAME LOGIC
bool CheckBoard()
{
	bool res = false;
	bool resC = false;
	for (int i = 0; i < active.amount * active.amount; i++)
	{
		gamed[i] = 0;
	}
	for (int i = 0; i < active.amount; i++)
	{
		res = res || CheckLine(i);
	}
	for (int i = 0; i < active.amount; i++)
	{
		resC = resC || CheckColumn(i);
	}
	return res || resC;
}

bool CheckLine(int i)
{
	bool res = false;;
	for (int c = 0; c < active.amount - 1; c++)
	{
		if (gamed[i * active.amount + c] == 1) continue;
		int k = c;
		Marked.push_back(gems[i * active.amount + c]);
		while (k < active.amount - 1)
		{
			if (colors[i * active.amount + k] == colors[i * active.amount + k + 1])
			{
				Marked.push_back(gems[i * active.amount + k + 1]);
				k++;
			}
			else
			{
				break;
			}

		}
		if (Marked.size() > 2)
		{
			for (auto gem : Marked)
			{
				gamed[IndexGem(gem)] = 1;
				res = true;
			}
		}
		Marked.clear();
	}
	return res;
}

bool CheckColumn(int i)
{
	bool res = false;
	for (int c = 0; c < active.amount - 1; c++)
	{
		if (gamed[active.amount * c + i] == 1) continue;
		int k = c;
		Marked.push_back(gems[active.amount * c + i]);
		while (k < active.amount - 1)
		{
			if (colors[active.amount * k + i] == colors[(k + 1) * active.amount + i])
			{
				Marked.push_back(gems[(k + 1) * active.amount + i]);
				k++;
			}
			else
			{
				break;
			}

		}
		if (Marked.size() > 2)
		{
			for (auto gem : Marked)
			{
				gamed[IndexGem(gem)] = 1;
				res = true;

			}
		}
		Marked.clear();
	}
	return res;
}

void MoveGemsDown(int j, int amount)
{
	HBRUSH* brushes = new HBRUSH[amount];
	for (int i = 0; i < amount; i++)
	{
		brushes[i] = colors[i * active.amount + j];
	}
	for (int i = amount; i > 0; i--)
	{
		colors[i * active.amount + j] = colors[(i - 1) * active.amount + j];
		InvalidateRect(gems[i * active.amount + j], NULL, TRUE);
		UpdateWindow(gems[i * active.amount + j]);
	}
	colors[j] = GetRandomColor();
	InvalidateRect(gems[j], NULL, TRUE);
	UpdateWindow(gems[j]);
	gamed[active.amount * amount + j] = 0;
} //undeclared

int GetFirstColumnDeletion(int j)
{
	int result = 0;
	int i = 0;
	while (gamed[i * active.amount + j] != 1 && i < active.amount)
	{
		result++;
		i++;
	}
	if (i == active.amount)
	{
		return -1;
	}
	return result;

}
bool something_to_be_deleted = false;
VOID Delete()
{ 
	something_to_be_deleted = false;
	for (int i = 0; i < active.amount * active.amount; i++)
	{
		if (gamed[i] == 1)
		{
			something_to_be_deleted = true;
			RECT rc;
			ToBeDeleted = gems[i];
			InvalidateRect(gems[i],NULL,TRUE);
			UpdateWindow(gems[i]);
			ToBeDeleted = NULL;
		}
	}
	if (something_to_be_deleted) {
		Sleep(700);
		int amount_of_steps = 0;
		for (int i = 0; i <active.amount; i++)
		{
			int row_count = 0;
			for (int j = 0; j < active.amount; j++)
			{
				if (gamed[i * active.amount + j] == 1)
				{
					row_count = 1;
				}
			}
			amount_of_steps += row_count;
		}

		for (int i = 0; i < amount_of_steps; i++)
		{
			for (int j = 0; j < active.amount; j++)
			{
				int deletion = GetFirstColumnDeletion(j);
				if (deletion == -1) continue;
				MoveGemsDown(j, deletion);
			}
			Sleep(500);
		}
	}
	while (something_to_be_deleted) {
		CheckBoard();
		Delete();
	}
}

LRESULT CALLBACK TopProc(HWND top, UINT message, WPARAM w, LPARAM l)
{
	switch (message)
	{
	case WM_ERASEBKGND:
	{
		return 1;
	}
	break;
	default:
		return DefWindowProc(top, message, w, l);
	}
}
LRESULT CALLBACK PartProc(HWND part, UINT message, WPARAM w, LPARAM l)
{
	switch (message)
	{
	default:
		return DefWindowProc(top, message, w, l);
	}
}