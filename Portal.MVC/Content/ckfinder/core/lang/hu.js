/*
 * CKFinder
 * ========
 * http://ckfinder.com
 * Copyright (C) 2007-2010, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 *
 * ---
 * Hungarian language file.
 */

var CKFLang =
{

Dir : 'ltr',
HelpLang : 'en',
LangCode : 'hu',

// Date Format
//		d    : Day
//		dd   : Day (padding zero)
//		m    : Month
//		mm   : Month (padding zero)
//		yy   : Year (two digits)
//		yyyy : Year (four digits)
//		h    : Hour (12 hour clock)
//		hh   : Hour (12 hour clock, padding zero)
//		H    : Hour (24 hour clock)
//		HH   : Hour (24 hour clock, padding zero)
//		M    : Minute
//		MM   : Minute (padding zero)
//		a    : Firt char of AM/PM
//		aa   : AM/PM
DateTime : 'yyyy. m. d. HH:MM',
DateAmPm : ['de.','du.'],

// Folders
FoldersTitle	: 'Mappák',
FolderLoading	: 'Betöltés...',
FolderNew		: 'Kérjük adja meg a mappa nevét: ',
FolderRename	: 'Kérjük adja meg a mappa új nevét: ',
FolderDelete	: 'Biztosan törölni szeretné a következő mappát: "%1"?',
FolderRenaming	: ' (átnevezés...)',
FolderDeleting	: ' (törlés...)',

// Files
FileRename		: 'Kérjük adja meg a fájl új nevét: ',
FileRenameExt	: 'Biztosan szeretné módosítani a fájl kiterjesztését? A fájl esetleg használhatatlan lesz.',
FileRenaming	: 'Átnevezés...',
FileDelete		: 'Biztosan törölni szeretné a következő fájlt: "%1"?',

// Toolbar Buttons (some used elsewhere)
Upload		: 'Feltöltés',
UploadTip	: 'Új fájl feltöltése',
Refresh		: 'Frissítés',
Settings	: 'Beállítások',
Help		: 'Súgó',
HelpTip		: 'Súgó (angolul)',

// Context Menus
Select		: 'Kiválaszt',
SelectThumbnail : 'Bélyegkép kiválasztása',
View		: 'Megtekintés',
Download	: 'Letöltés',

NewSubFolder	: 'Új almappa',
Rename			: 'Átnevezés',
Delete			: 'Törlés',

// Generic
OkBtn		: 'OK',
CancelBtn	: 'Mégsem',
CloseBtn	: 'Bezárás',

// Upload Panel
UploadTitle			: 'Új fájl feltöltése',
UploadSelectLbl		: 'Válassza ki a feltölteni kívánt fájlt',
UploadProgressLbl	: '(A feltöltés folyamatban, kérjük várjon...)',
UploadBtn			: 'A kiválasztott fájl feltöltése',

UploadNoFileMsg		: 'Kérjük válassza ki a fájlt a számítógépéről',

// Settings Panel
SetTitle		: 'Beállítások',
SetView			: 'Nézet:',
SetViewThumb	: 'bélyegképes',
SetViewList		: 'listás',
SetDisplay		: 'Megjelenik:',
SetDisplayName	: 'fájl neve',
SetDisplayDate	: 'dátum',
SetDisplaySize	: 'fájlméret',
SetSort			: 'Rendezés:',
SetSortName		: 'fájlnév',
SetSortDate		: 'dátum',
SetSortSize		: 'méret',

// Status Bar
FilesCountEmpty : '<üres mappa>',
FilesCountOne	: '1 fájl',
FilesCountMany	: '%1 fájl',

// Size and Speed
Kb				: '%1 kB',
KbPerSecond		: '%1 kB/s',

// Connector Error Messages.
ErrorUnknown : 'A parancsot nem sikerült végrehajtani. (Hiba: %1)',
Errors :
{
 10 : 'Érvénytelen parancs.',
 11 : 'A fájl típusa nem lett a kérés során beállítva.',
 12 : 'A kívánt fájl típus érvénytelen.',
102 : 'Érvénytelen fájl vagy könyvtárnév.',
103 : 'Hitelesítési problémák miatt nem sikerült a kérést teljesíteni.',
104 : 'Jogosultsági problémák miatt nem sikerült a kérést teljesíteni.',
105 : 'Érvénytelen fájl kiterjesztés.',
109 : 'Érvénytelen kérés.',
110 : 'Ismeretlen hiba.',
115 : 'A fálj vagy mappa már létezik ezen a néven.',
116 : 'Mappa nem található. Kérjük frissítsen és próbálja újra.',
117 : 'Fájl nem található. Kérjük frissítsen és próbálja újra.',
201 : 'Ilyen nevű fájl már létezett. A feltöltött fájl a következőre lett átnevezve: "%1"',
202 : 'Érvénytelen fájl',
203 : 'Érvénytelen fájl. A fájl mérete túl nagy.',
204 : 'A feltöltött fájl hibás.',
205 : 'A szerveren nem található a feltöltéshez ideiglenes mappa.',
206 : 'A feltöltés biztonsági okok miatt meg lett szakítva. The file contains HTML like data.',
207 : 'A feltöltött fájl a következőre lett átnevezve: "%1"',
500 : 'A fájl-tallózó biztonsági okok miatt nincs engedélyezve. Kérjük vegye fel a kapcsolatot a rendszer üzemeltetőjével és ellenőrizze a CKFinder konfigurációs fájlt.',
501 : 'A bélyegkép támogatás nincs engedélyezve.'
},

// Other Error Messages.
ErrorMsg :
{
FileEmpty		: 'A fájl neve nem lehet üres',
FolderEmpty		: 'A mappa neve nem lehet üres',

FileInvChar		: 'A fájl neve nem tartalmazhatja a következő karaktereket: \n\\ / : * ? " < > |',
FolderInvChar	: 'A mappa neve nem tartalmazhatja a következő karaktereket: \n\\ / : * ? " < > |',

PopupBlockView	: 'A felugró ablak megnyitása nem sikerült. Kérjük ellenőrizze a böngészője beállításait és tiltsa le a felugró ablakokat blokkoló alkalmazásait erre a honlapra.'
}

} ;
