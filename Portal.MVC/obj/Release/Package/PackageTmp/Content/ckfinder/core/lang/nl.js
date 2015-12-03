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
 * Dutch language file.
 */

var CKFLang =
{

Dir : 'ltr',
HelpLang : 'en',
LangCode : 'nl',

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
FoldersTitle	: 'Directory',
FolderLoading	: 'Laden...',
FolderNew		: 'Vul de mapnaam in: ',
FolderRename	: 'Vul de nieuwe mapnaam in: ',
FolderDelete	: 'Weet je het zeker dat je de map: "%1" wilt verwijderen?',
FolderRenaming	: ' (Aanpassen...)',
FolderDeleting	: ' (Verwijderen...)',

// Files
FileRename		: 'Vul de nieuwe naam in: ',
FileRenameExt	: 'Weet je zeker dat je de extentie wilt veranderen? Het kan zijn dat het bestand niet meer werkt!',
FileRenaming	: 'Aanpassen...',
FileDelete		: 'Weet je zeker dat je bestand: "%1" wilt verwijderen?',

// Toolbar Buttons (some used elsewhere)
Upload		: 'Uploaden',
UploadTip	: 'Nieuw bestand uploaden',
Refresh		: 'Vernieuwen',
Settings	: 'Instellingen',
Help		: 'Help',
HelpTip		: 'Help',

// Context Menus
Select		: 'Selecteer',
SelectThumbnail : 'Selecteer Mini-afbeelding',
View		: 'Bekijken',
Download	: 'Downloaden',

NewSubFolder	: 'Nieuwe sub-folder',
Rename			: 'Naam aanpassen',
Delete			: 'Verwijderen',

// Generic
OkBtn		: 'OK',
CancelBtn	: 'Annuleren',
CloseBtn	: 'Sluiten',

// Upload Panel
UploadTitle			: 'Nieuw bestand uploaden',
UploadSelectLbl		: 'Selecteer het bestand om te uploaden',
UploadProgressLbl	: '(Bezig met uploaden, even geduld...)',
UploadBtn			: 'Upload geselecteerde bestand',

UploadNoFileMsg		: 'Kies een bestand van je computer.',

// Settings Panel
SetTitle		: 'Instellingen',
SetView			: 'Bekijken:',
SetViewThumb	: 'Mini-afbeelding',
SetViewList		: 'Lijst',
SetDisplay		: 'Scherm:',
SetDisplayName	: 'Bestand naam',
SetDisplayDate	: 'Datum',
SetDisplaySize	: 'Bestand grootte',
SetSort			: 'Sorteren op:',
SetSortName		: 'Bij bestandsnaam',
SetSortDate		: 'Bij datum',
SetSortSize		: 'Bij grote',

// Status Bar
FilesCountEmpty : '<Lege directory>',
FilesCountOne	: '1 bestand',
FilesCountMany	: '%1 bestand',

// Size and Speed
Kb				: '%1 kB',
KbPerSecond		: '%1 kB/s',

// Connector Error Messages.
ErrorUnknown : 'Het was niet mogelijk om deze actie uit te voeren. (Fout %1)',
Errors :
{
 10 : 'Verkeerd commando.',
 11 : 'De bestand typen komt niet voor in de aanvraag.',
 12 : 'De gevraagde resource type is niet geldig.',
102 : 'Foute bestand of folder naam.',
103 : 'Het was niet mogelijk om verzoek te doen naar de authorization restrictions.',
104 : 'Het was niet mogelijk om toegang te krijgen tot het permissie systeem.',
105 : 'Deze extentie mag niet.',
109 : 'Verkeerde aanvraag.',
110 : 'Onbekende fout.',
115 : 'Er bestaat al een bestand of folder met deze naam.',
116 : 'Folder niet gevonden, vernieuw het systeem of kies een andere folder.',
117 : 'Bestand niet gevonden, vernieuw het systeem of kies een andere folder.',
201 : 'Een bestand met de zelfde naam is er al. Het geuploade bestand is hernoemd naar: "%1"',
202 : 'Verkeerde bestand',
203 : 'Verkeerde bestand. Het bestand is te groot.',
204 : 'De geuploade file is kapot.',
205 : 'Er is geen hoofd folder gevonden.',
206 : 'Uploaden van de file is afgebroken, er is html in het bestand aangetroffen.',
207 : 'Het geuploade bestand is hernoemd naar: "%1"',
500 : 'Het uploaden van een file is momenteel niet mogelijk. Contacteer de admin of kijk in het CKFinder configuratie bestand..',
501 : 'Het is niet mogelijk om mini-afbeeldingen te maken.'
},

// Other Error Messages.
ErrorMsg :
{
FileEmpty		: 'Je dient een naam te geven aan dit bestand',
FolderEmpty		: 'Je dient een naam te geven aan deze folder',

FileInvChar		: 'De bestandsnaam mag de volgende tekens NIET bevatten: \n\\ / : * ? " < > |',
FolderInvChar	: 'De folder mag de volgende tekens NIET bevatten: \n\\ / : * ? " < > |',

PopupBlockView	: 'Het was niet mogelijk om dit bestand in een popup te openen. Kijk bij je instellingen van je browser en zorg dat het onze popups niet blokkeerd.'
}

} ;

