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
 * Czech language file.
 */

var CKFLang =
{

Dir : 'ltr',
HelpLang : 'en',
LangCode : 'cs',

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
DateTime : 'm/d/yyyy h:MM aa',
DateAmPm : ['AM','PM'],

// Folders
FoldersTitle	: 'Složky',
FolderLoading	: 'Načítání...',
FolderNew		: 'Zadejte jméno nové složky: ',
FolderRename	: 'Zadejte nové jméno složky: ',
FolderDelete	: 'Opravdu chcete smazat složku "%1" ?',
FolderRenaming	: ' (Přejmenovávám...)',
FolderDeleting	: ' (Mažu...)',

// Files
FileRename		: 'Zadejte jméno novéhho souboru: ',
FileRenameExt	: 'Opravdu chcete změnit příponu souboru, může se stát nečitelným',
FileRenaming	: 'Přejmenovávám...',
FileDelete		: 'Opravdu chcete smazat soubor "%1"?',

// Toolbar Buttons (some used elsewhere)
Upload		: 'Nahrát',
UploadTip	: 'Nahrát nový soubor',
Refresh		: 'Načíst znova',
Settings	: 'Nastavení',
Help		: 'Pomoc',
HelpTip		: 'Pomoc',

// Context Menus
Select		: 'Vybrat',
SelectThumbnail : 'Vybrat náhled',
View		: 'Zobrazit',
Download	: 'Uložit jako',

NewSubFolder	: 'Nová podsložka',
Rename			: 'Přejmenovat',
Delete			: 'Smazat',

// Generic
OkBtn		: 'OK',
CancelBtn	: 'Zrušit',
CloseBtn	: 'Zavřít',

// Upload Panel
UploadTitle			: 'Nahrát nový soubor',
UploadSelectLbl		: 'Zvolit soubor k nahrání',
UploadProgressLbl	: '(Nahrávám, čekejte...)',
UploadBtn			: 'Nahrát zvolený soubor',

UploadNoFileMsg		: 'Vyberte prosím soubor',

// Settings Panel
SetTitle		: 'Nastavení',
SetView			: 'Zobrazení:',
SetViewThumb	: 'Náhledy',
SetViewList		: 'Seznam',
SetDisplay		: 'Informace:',
SetDisplayName	: 'Název',
SetDisplayDate	: 'Datum',
SetDisplaySize	: 'Velikost',
SetSort			: 'Seřazení:',
SetSortName		: 'Podle jména',
SetSortDate		: 'Podle data',
SetSortSize		: 'Podle velikosti',

// Status Bar
FilesCountEmpty : '<Prázdná složka>',
FilesCountOne	: '1 soubor',
FilesCountMany	: '%1 soubor',

// Size and Speed
Kb				: '%1 kB',
KbPerSecond		: '%1 kB/s',

// Connector Error Messages.
ErrorUnknown : 'Nebylo možno dokončit příkaz. (Error %1)',
Errors :
{
 10 : 'Neplatný příkaz.',
 11 : 'Požadovaný typ prostředku nebyl specifikován v dotazu.',
 12 : 'Požadovaný typ prostředku není validní.',
102 : 'Šatné jméno souboru, nebo složky.',
103 : 'Nebylo možné dokončit příkaz kvůli autorizačním omezením.',
104 : 'Nebylo možné dokončit příkaz kvůli omezeným přístupovým právům k souborům.',
105 : 'Špatná přípona souboru.',
109 : 'Neplatný příkaz.',
110 : 'Neznámá chyba.',
115 : 'Již existuje soubor nebo složka se stejným jménem.',
116 : 'Složka nenalezena, prosím obnovte stránku.',
117 : 'Soubor nenalezen, prosím obnovte stránku.',
201 : 'Již existoval soubor se stejným jménem, nahraný soubor byl přejmenován na "%1"',
202 : 'Špatný soubor',
203 : 'Špatný soubor. Příliš velký.',
204 : 'Nahraný soubor je poškozen.',
205 : 'Na serveru není dostupná dočasná složka.',
206 : 'Nahrávání zrušeno z bezpečnostních důvodů. Soubor obsahuje data podobná HTML.',
207 : 'Nahraný soubor byl přejmenován na "%1"',
500 : 'Nahrávání zrušeno z bezpečnostních důvodů. Zdělte to prosím administrátorovi a zkontrolujte nastavení CKFinderu.',
501 : 'Podpora náhledů je vypnuta.'
},

// Other Error Messages.
ErrorMsg :
{
FileEmpty		: 'Název souboru nemůže být prázdný',
FolderEmpty		: 'Název složky nemůže být prázdný',

FileInvChar		: 'Název souboru nesmí obsahovat následující znaky: \n\\ / : * ? " < > |',
FolderInvChar	: 'Název složky nesmí obsahovat následující znaky: \n\\ / : * ? " < > |',

PopupBlockView	: 'Nebylo možné otevřít soubor do nového okna. Prosím nastavte si prohlížeč aby neblokoval vyskakovací okna.'
}

} ;
