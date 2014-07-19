/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: Options.cs,v 1.9 2012/05/27 15:22:50 kzmi Exp $
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using Poderosa.Util;
using Poderosa.Document;
using Poderosa.View;
using Poderosa.ConnectionParam;
using Poderosa.Terminal;
using Poderosa.Preferences;
using Poderosa.Sessions;

//�N���̍������̂��߁A�����ł�Granados���Ă΂Ȃ��悤�ɒ��ӂ���

namespace Poderosa.Terminal {
    /// <summary>
    /// <ja>
    /// �^�[�~�i���G�~�����[�^�̃I�v�V�����������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows option of terminal emulator
    /// </en>
    /// </summary>
    /// <exclude/>
    public interface ITerminalEmulatorOptions {
        /// <summary>
        /// <ja>�t�H���g�������܂��B</ja><en>The font is shown. </en>
        /// </summary>
        Font Font {
            get;
            set;
        }
        /// <summary>
        /// <ja>CJK�t�H���g�������܂�</ja>
        /// <en>A CJK font is shown. </en>
        /// </summary>
        Font CJKFont {
            get;
            set;
        }

        /// <summary>
        /// <ja>�ؒf���ꂽ�Ƃ��ɕ��邩�ǂ����̃t���O�ł��B</ja>
        /// <en>Flag whether close when closed</en>
        /// </summary>
        bool CloseOnDisconnect {
            get;
            set;
        }

        /// <summary>
        /// <ja>�x���L�������������Ƃ��Ƀr�[�v��炷���ǂ����̃t���O�ł��B</ja>
        /// <en>Flag whether to sound beep when the bell character comes. </en>
        /// </summary>
        bool BeepOnBellChar {
            get;
            set;
        }

        /// <summary>
        /// <ja>�{�[���h���������邩�ǂ����̃t���O</ja>
        /// <en>Flag whether force bold style</en>
        /// </summary>
        bool ForceBoldStyle {
            get;
            set;
        }

        /// <summary>
        /// <ja>�s�ԃX�y�[�X (�s�N�Z��)</ja>
        /// <en>Line spacing (pixels)</en>
        /// </summary>
        int LineSpacing {
            get;
            set;
        }

        //�L�[�ݒ�n
        /// <summary>
        /// <ja>DEL�L�[��0x7F�R�[�h�𑗐M���邩�ǂ����̃t���O�ł��B</ja>
        /// <en>Flag whether to transmit 0x7F code with DEL key.</en>
        /// </summary>
        bool Send0x7FByDel {
            get;
            set;
        }
        bool Send0x7FByBack {
            get;
            set;
        }

        KeyboardStyle Zone0x1F {
            get;
            set;
        }
        string CustomKeySettings {
            get;
            set;
        }

        WarningOption CharDecodeErrorBehavior {
            get;
            set;
        }
        WarningOption DisconnectNotification {
            get;
            set;
        }

        bool AllowsScrollInAppMode {
            get;
            set;
        }
        string AdditionalWordElement {
            get;
            set;
        }
        int WheelAmount {
            get;
            set;
        }

        int TerminalBufferSize {
            get;
            set;
        }


        bool UseClearType {
            get;
            set;
        }
        bool EnableBoldStyle {
            get;
            set;
        }
        int KeepAliveInterval {
            get;
            set;
        }

        Color BGColor {
            get;
            set;
        }
        Color TextColor {
            get;
            set;
        }
        EscapesequenceColorSet EscapeSequenceColorSet {
            get;
            set;
        }
        bool DarkenEsColorForBackground {
            get;
            set;
        }
        string BackgroundImageFileName {
            get;
            set;
        }
        ImageStyle ImageStyle {
            get;
            set;
        }

        AltKeyAction LeftAltKey {
            get;
            set;
        }
        AltKeyAction RightAltKey {
            get;
            set;
        }
        MouseButtonAction RightButtonAction {
            get;
            set;
        }
        MouseButtonAction MiddleButtonAction {
            get;
            set;
        }
        LogType DefaultLogType {
            get;
            set;
        }
        string DefaultLogDirectory {
            get;
            set;
        }
        CaretType CaretType {
            get;
            set;
        }
        Color CaretColor {
            get;
            set;
        }
        bool CaretBlink {
            get;
            set;
        }

        //ShellSupport�����
        bool EnableComplementForNewConnections {
            get;
            set;
        }
        bool CommandPopupAlwaysOnTop {
            get;
            set;
        }
        bool CommandPopupInTaskBar {
            get;
            set;
        }


        // Copy and Paste

        /// <summary>
        /// <ja>���s�������y�[�X�g����鎞�Ɍx����\�����邩�ǂ���</ja>
        /// <en>Setting whether alert is shown when a new-line character will be pasted</en>
        /// </summary>
        bool AlertOnPasteNewLineChar {
            get;
            set;
        }


        //PreferenceEditor�݂̂ŕҏW�\
        Keys IntelliSenseKey {
            get;
        }
        Keys CommandPopupKey {
            get;
        }
        int ShellHistoryLimitCount {
            get;
        }


        RenderProfile CreateRenderProfile(); //NOTE ����͈Ӗ��I�ɂ�����Ƃ܂��������BPreference�ɓ������ׂ���
    }

    internal class TerminalOptions : SnapshotAwarePreferenceBase, ITerminalEmulatorOptions {
        //�\��
        private IStringPreferenceItem _fontName;
        private IStringPreferenceItem _cjkFontName;
        private IIntPreferenceItem _fontSize; //float�ɂ��ׂ����Ȃ�
        private IBoolPreferenceItem _useClearType;
        private IBoolPreferenceItem _enableBoldStyle;
        private IBoolPreferenceItem _forceBoldStyle;
        private ColorPreferenceItem _bgColor;
        private ColorPreferenceItem _textColor;
        private IIntPreferenceItem _lineSpacing;
        /* Special Handlings Required */
        private IStringPreferenceItem _backgroundImageFileName;
        private EnumPreferenceItem<ImageStyle> _imageStyle;
        private IStringPreferenceItem _escapeSequenceColorSet;
        private IBoolPreferenceItem _darkenEsColorForBackground;

        private ColorPreferenceItem _caretColor;
        private EnumPreferenceItem<CaretType> _caretType;
        private IBoolPreferenceItem _caretBlink;

        //�^�[�~�i��
        private IBoolPreferenceItem _closeOnDisconnect;
        private IBoolPreferenceItem _beepOnBellChar;
        private IBoolPreferenceItem _askCloseOnExit;
        private EnumPreferenceItem<WarningOption> _charDecodeErrorBehavior;
        private EnumPreferenceItem<WarningOption> _disconnectNotification;
        //Shell Support
        private IBoolPreferenceItem _enableComplementForNewConnections;
        private IBoolPreferenceItem _commandPopupAlwaysOnTop;
        private IBoolPreferenceItem _commandPopupInTaskBar;


        //����
        private IIntPreferenceItem _terminalBufferSize;
        private IBoolPreferenceItem _send0x7FByDel;
        private IBoolPreferenceItem _send0x7FByBack;
        private EnumPreferenceItem<KeyboardStyle> _zone0x1F;
        private IStringPreferenceItem _customKeySettings;
        private IBoolPreferenceItem _allowsScrollInAppMode;
        private IIntPreferenceItem _keepAliveInterval;
        private IStringPreferenceItem _additionalWordElement;

        //�}�E�X�ƃL�[�{�[�h
        private IIntPreferenceItem _wheelAmount;
        private EnumPreferenceItem<AltKeyAction> _leftAltKey;
        private EnumPreferenceItem<AltKeyAction> _rightAltKey;
        private EnumPreferenceItem<MouseButtonAction> _rightButtonAction;
        private EnumPreferenceItem<MouseButtonAction> _middleButtonAction;

        //���O
        private EnumPreferenceItem<LogType> _defaultLogType;
        private IStringPreferenceItem _defaultLogDirectory;

        // Copy and Paste
        private IBoolPreferenceItem _alertOnPasteNewLineChar;

        //PreferenceEditor�̂�
        private bool _parseKeyRequired;
        private Keys _intelliSenseKeyCache;
        private Keys _commandPopupKeyCache;
        private IStringPreferenceItem _intelliSenseKey;
        private IStringPreferenceItem _commandPopupKey;
        private IIntPreferenceItem _shellHistoryLimitCount;

        //���̂Q�͏�L�v�f����쐬
        private Font _font;
        private Font _cjkFont;


        public TerminalOptions(IPreferenceFolder folder)
            : base(folder) {
        }

        public override void DefineItems(IPreferenceBuilder builder) {
            //�\��
            _fontName = builder.DefineStringValue(_folder, "fontName", "Courier New", null);
            _cjkFontName = builder.DefineStringValue(_folder, "cjkFontName", "�l�r �S�V�b�N", null);
            _fontSize = builder.DefineIntValue(_folder, "fontSize", 10, PreferenceValidatorUtil.PositiveIntegerValidator); //float�ɂ��ׂ����Ȃ�
            _useClearType = builder.DefineBoolValue(_folder, "useClearType", true, null);
            _enableBoldStyle = builder.DefineBoolValue(_folder, "enableBoldStyle", true, null);
            _forceBoldStyle = builder.DefineBoolValue(_folder, "forceBoldStyle", false, null);
            _lineSpacing = builder.DefineIntValue(_folder, "lineSpacing", 0, PreferenceValidatorUtil.IntRangeValidator(0, 10));
            _bgColor = new ColorPreferenceItem(builder.DefineStringValue(_folder, "bgColor", "Window", null), KnownColor.Window);
            _textColor = new ColorPreferenceItem(builder.DefineStringValue(_folder, "textColor", "WindowText", null), KnownColor.WindowText);
            _backgroundImageFileName = builder.DefineStringValue(_folder, "backgroundImageFileName", "", null);
            _imageStyle = new EnumPreferenceItem<ImageStyle>(builder.DefineStringValue(_folder, "imageStyle", "Center", null), ImageStyle.Center);
            _escapeSequenceColorSet = builder.DefineStringValue(_folder, "escapeSequenceColorSet", "", null);
            _darkenEsColorForBackground = builder.DefineBoolValue(_folder, "darkenEsColorForBackground", true, null);

            _caretColor = new ColorPreferenceItem(builder.DefineStringValue(_folder, "caretColor", "Empty", null), Color.Empty);
            _caretType = new EnumPreferenceItem<CaretType>(builder.DefineStringValue(_folder, "caretType", "Box", null), CaretType.Box);
            _caretBlink = builder.DefineBoolValue(_folder, "caretBlink", true, null);

            //�^�[�~�i��
            _closeOnDisconnect = builder.DefineBoolValue(_folder, "closeOnDisconnect", true, null);
            _beepOnBellChar = builder.DefineBoolValue(_folder, "beepOnBellChar", false, null);
            _askCloseOnExit = builder.DefineBoolValue(_folder, "askCloseOnExit", true, null);
            _charDecodeErrorBehavior = new EnumPreferenceItem<WarningOption>(builder.DefineStringValue(_folder, "charDecodeErrorBehavior", "MessageBox", null), WarningOption.MessageBox);
            _disconnectNotification = new EnumPreferenceItem<WarningOption>(builder.DefineStringValue(_folder, "disconnectNotification", "StatusBar", null), WarningOption.StatusBar);
            _enableComplementForNewConnections = builder.DefineBoolValue(_folder, "enableComplementForNewConnections", false, null);
            _commandPopupAlwaysOnTop = builder.DefineBoolValue(_folder, "commandPopupAlwaysOnTop", false, null);
            _commandPopupInTaskBar = builder.DefineBoolValue(_folder, "commandPopupInTaskBar", false, null);

            //����
            _terminalBufferSize = builder.DefineIntValue(_folder, "terminalBufferSize", 1000, PreferenceValidatorUtil.PositiveIntegerValidator);
            _send0x7FByDel = builder.DefineBoolValue(_folder, "send0x7FByDel", false, null);
            _send0x7FByBack = builder.DefineBoolValue(_folder, "send0x7FByBack", false, null);
            _zone0x1F = new EnumPreferenceItem<KeyboardStyle>(builder.DefineStringValue(_folder, "zone0x1F", "None", null), KeyboardStyle.None);
            _customKeySettings = builder.DefineStringValue(_folder, "customKeySettings", "", null);
            _allowsScrollInAppMode = builder.DefineBoolValue(_folder, "allowsScrollInAppMode", false, null);
            _keepAliveInterval = builder.DefineIntValue(_folder, "keepAliveInterval", 60000, PreferenceValidatorUtil.IntRangeValidator(0, 100 * 60000));
            _additionalWordElement = builder.DefineStringValue(_folder, "additionalWordElement", "", null);

            //�}�E�X�ƃL�[�{�[�h
            _wheelAmount = builder.DefineIntValue(_folder, "wheelAmount", 3, PreferenceValidatorUtil.PositiveIntegerValidator);
            _leftAltKey = new EnumPreferenceItem<AltKeyAction>(builder.DefineStringValue(_folder, "leftAltKey", "Menu", null), AltKeyAction.Menu);
            _rightAltKey = new EnumPreferenceItem<AltKeyAction>(builder.DefineStringValue(_folder, "rightAltKey", "Menu", null), AltKeyAction.Menu);
            _rightButtonAction = new EnumPreferenceItem<MouseButtonAction>(builder.DefineStringValue(_folder, "rightButtonAction", "ContextMenu", null), MouseButtonAction.ContextMenu);
            _middleButtonAction = new EnumPreferenceItem<MouseButtonAction>(builder.DefineStringValue(_folder, "middleButtonAction", "None", null), MouseButtonAction.None);

            //���O
            _defaultLogType = new EnumPreferenceItem<LogType>(builder.DefineStringValue(_folder, "defaultLogType", "None", null), LogType.None);
            _defaultLogDirectory = builder.DefineStringValue(_folder, "defaultLogDirectory", "", null);

            // Copy and Paste
            _alertOnPasteNewLineChar = builder.DefineBoolValue(_folder, "alertOnPasteNewLineChar", true, null);

            //PreferenceEditor�̂�
            _intelliSenseKey = builder.DefineStringValue(_folder, "intelliSenseKey", "Ctrl+OemPeriod", PreferenceValidatorUtil.KeyWithModifierValidator);
            _commandPopupKey = builder.DefineStringValue(_folder, "commandPopupKey", "Ctrl+Oemcomma", PreferenceValidatorUtil.KeyWithModifierValidator);
            _parseKeyRequired = true;
            _shellHistoryLimitCount = builder.DefineIntValue(_folder, "shellHistoryLimitCount", 100, PreferenceValidatorUtil.PositiveIntegerValidator);
        }
        public TerminalOptions Import(TerminalOptions src) {
            //�\��
            _fontName = ConvertItem(src._fontName);
            _cjkFontName = ConvertItem(src._cjkFontName);
            _fontSize = ConvertItem(src._fontSize); //float�ɂ��ׂ����Ȃ�
            _useClearType = ConvertItem(src._useClearType);
            _enableBoldStyle = ConvertItem(src._enableBoldStyle);
            _forceBoldStyle = ConvertItem(src._forceBoldStyle);
            _lineSpacing = ConvertItem(src._lineSpacing);
            _bgColor = ConvertItem(src._bgColor);
            _textColor = ConvertItem(src._textColor);
            _backgroundImageFileName = ConvertItem(src._backgroundImageFileName);
            _imageStyle = ConvertItem<ImageStyle>(src._imageStyle);
            _escapeSequenceColorSet = ConvertItem(src._escapeSequenceColorSet);
            _darkenEsColorForBackground = ConvertItem(src._darkenEsColorForBackground);

            _caretColor = ConvertItem(src._caretColor);
            _caretType = ConvertItem<CaretType>(src._caretType);
            _caretBlink = ConvertItem(src._caretBlink);

            //�^�[�~�i��
            _closeOnDisconnect = ConvertItem(src._closeOnDisconnect);
            _beepOnBellChar = ConvertItem(src._beepOnBellChar);
            _askCloseOnExit = ConvertItem(src._askCloseOnExit);
            _charDecodeErrorBehavior = ConvertItem<WarningOption>(src._charDecodeErrorBehavior);
            _disconnectNotification = ConvertItem<WarningOption>(src._disconnectNotification);
            _enableComplementForNewConnections = ConvertItem(src._enableComplementForNewConnections);
            _commandPopupAlwaysOnTop = ConvertItem(src._commandPopupAlwaysOnTop);
            _commandPopupInTaskBar = ConvertItem(src._commandPopupInTaskBar);

            //����
            _terminalBufferSize = ConvertItem(src._terminalBufferSize);
            _send0x7FByDel = ConvertItem(src._send0x7FByDel);
            _send0x7FByBack = ConvertItem(src._send0x7FByBack);
            _zone0x1F = ConvertItem<KeyboardStyle>(src._zone0x1F);
            _customKeySettings = ConvertItem(src._customKeySettings);
            _allowsScrollInAppMode = ConvertItem(src._allowsScrollInAppMode);
            _keepAliveInterval = ConvertItem(src._keepAliveInterval);
            _additionalWordElement = ConvertItem(src._additionalWordElement);

            //�}�E�X�ƃL�[�{�[�h
            _wheelAmount = ConvertItem(src._wheelAmount);
            _leftAltKey = ConvertItem<AltKeyAction>(src._leftAltKey);
            _rightAltKey = ConvertItem<AltKeyAction>(src._rightAltKey);
            _rightButtonAction = ConvertItem<MouseButtonAction>(src._rightButtonAction);
            _middleButtonAction = ConvertItem<MouseButtonAction>(src._middleButtonAction);

            //���O
            _defaultLogType = ConvertItem<LogType>(src._defaultLogType);
            _defaultLogDirectory = ConvertItem(src._defaultLogDirectory);

            // Copy and Paste
            _alertOnPasteNewLineChar = ConvertItem(src._alertOnPasteNewLineChar);

            //PreferenceEditor�̂�
            _intelliSenseKey = ConvertItem(src._intelliSenseKey);
            _commandPopupKey = ConvertItem(src._commandPopupKey);
            _parseKeyRequired = true;
            _shellHistoryLimitCount = ConvertItem(src._shellHistoryLimitCount);

            return this;
        }

        public Font Font {
            get {
                if (_font == null) {
                    _font = RuntimeUtil.CreateFont(_fontName.Value, GetFontSizeAsFloat());
                }
                return _font;
            }
            set { //�T�C�Y�̕ύX�͂�������
                _font = value;
                _fontName.Value = GetFontName(value);
                _fontSize.Value = (int)value.Size;
            }
        }

        public Font CJKFont {
            get {
                if (_cjkFont == null)
                    _cjkFont = RuntimeUtil.CreateFont(_cjkFontName.Value, GetFontSizeAsFloat());
                return _cjkFont;
            }
            set {
                _cjkFont = value;
                _cjkFontName.Value = GetFontName(value);
            }
        }

        private String GetFontName(Font font) {
            // If Font.OriginalFontName property was available,
            // we use it to keep the font name in the config file.
            PropertyInfo prop = typeof(Font).GetProperty("OriginalFontName", BindingFlags.Instance | BindingFlags.Public);
            if (prop != null) {
                String fontName = prop.GetValue(font, null) as String;
                if (fontName != null)
                    return fontName;
            }
            return font.FontFamily.Name;
        }

        public bool CloseOnDisconnect {
            get {
                return _closeOnDisconnect.Value;
            }
            set {
                _closeOnDisconnect.Value = value;
            }
        }
        public bool BeepOnBellChar {
            get {
                return _beepOnBellChar.Value;
            }
            set {
                _beepOnBellChar.Value = value;
            }
        }

        public bool Send0x7FByDel {
            get {
                return _send0x7FByDel.Value;
            }
            set {
                _send0x7FByDel.Value = value;
            }
        }
        public bool Send0x7FByBack {
            get {
                return _send0x7FByBack.Value;
            }
            set {
                _send0x7FByBack.Value = value;
            }
        }

        public KeyboardStyle Zone0x1F {
            get {
                return _zone0x1F.Value;
            }
            set {
                _zone0x1F.Value = value;
            }
        }
        public string CustomKeySettings {
            get {
                return _customKeySettings.Value;
            }
            set {
                _customKeySettings.Value = value;
            }
        }


        public bool AllowsScrollInAppMode {
            get {
                return _allowsScrollInAppMode.Value;
            }
            set {
                _allowsScrollInAppMode.Value = value;
            }
        }

        public string AdditionalWordElement {
            get {
                return _additionalWordElement.Value;
            }
            set {
                _additionalWordElement.Value = value;
            }
        }

        public int WheelAmount {
            get {
                return _wheelAmount.Value;
            }
            set {
                _wheelAmount.Value = value;
            }
        }

        public int TerminalBufferSize {
            get {
                return _terminalBufferSize.Value;
            }
            set {
                _terminalBufferSize.Value = value;
            }
        }

        public bool UseClearType {
            get {
                return _useClearType.Value;
            }
            set {
                _useClearType.Value = value;
            }
        }
        public bool EnableBoldStyle {
            get {
                return _enableBoldStyle.Value;
            }
            set {
                _enableBoldStyle.Value = value;
            }
        }

        public bool ForceBoldStyle {
            get {
                return _forceBoldStyle.Value;
            }
            set {
                _forceBoldStyle.Value = value;
            }
        }

        public int LineSpacing {
            get {
                return _lineSpacing.Value;
            }
            set {
                _lineSpacing.Value = value;
            }
        }

        public int KeepAliveInterval {
            get {
                return _keepAliveInterval.Value;
            }
            set {
                _keepAliveInterval.Value = value;
            }
        }


        public string BackgroundImageFileName {
            get {
                return _backgroundImageFileName.Value;
            }
            set {
                _backgroundImageFileName.Value = value;
            }
        }

        public Color BGColor {
            get {
                return _bgColor.Value;
            }
            set {
                _bgColor.Value = value;
            }
        }
        public Color TextColor {
            get {
                return _textColor.Value;
            }
            set {
                _textColor.Value = value;
            }
        }
        public AltKeyAction LeftAltKey {
            get {
                return _leftAltKey.Value;
            }
            set {
                _leftAltKey.Value = value;
            }
        }
        public AltKeyAction RightAltKey {
            get {
                return _rightAltKey.Value;
            }
            set {
                _rightAltKey.Value = value;
            }
        }
        public MouseButtonAction RightButtonAction {
            get {
                return _rightButtonAction.Value;
            }
            set {
                _rightButtonAction.Value = value;
            }
        }
        public MouseButtonAction MiddleButtonAction {
            get {
                return _middleButtonAction.Value;
            }
            set {
                _middleButtonAction.Value = value;
            }
        }
        public LogType DefaultLogType {
            get {
                return _defaultLogType.Value;
            }
            set {
                _defaultLogType.Value = value;
            }
        }
        public string DefaultLogDirectory {
            get {
                return _defaultLogDirectory.Value;
            }
            set {
                _defaultLogDirectory.Value = value;
            }
        }
        public ImageStyle ImageStyle {
            get {
                return _imageStyle.Value;
            }
            set {
                _imageStyle.Value = value;
            }
        }
        public CaretType CaretType {
            get {
                return _caretType.Value;
            }
            set {
                _caretType.Value = value;
            }
        }
        public Color CaretColor {
            get {
                return _caretColor.Value;
            }
            set {
                _caretColor.Value = value;
            }
        }
        public bool CaretBlink {
            get {
                return _caretBlink.Value;
            }
            set {
                _caretBlink.Value = value;
            }
        }
        public WarningOption CharDecodeErrorBehavior {
            get {
                return _charDecodeErrorBehavior.Value;
            }
            set {
                _charDecodeErrorBehavior.Value = value;
            }
        }
        public WarningOption DisconnectNotification {
            get {
                return _disconnectNotification.Value;
            }
            set {
                _disconnectNotification.Value = value;
            }
        }
        public EscapesequenceColorSet EscapeSequenceColorSet {
            get {
                return EscapesequenceColorSet.Parse(_escapeSequenceColorSet.Value);
            }
            set {
                _escapeSequenceColorSet.Value = value.Format();
            }
        }
        public bool DarkenEsColorForBackground {
            get {
                return _darkenEsColorForBackground.Value;
            }
            set {
                _darkenEsColorForBackground.Value = value;
            }
        }
        public bool EnableComplementForNewConnections {
            get {
                return _enableComplementForNewConnections.Value;
            }
            set {
                _enableComplementForNewConnections.Value = value;
            }
        }
        public bool CommandPopupAlwaysOnTop {
            get {
                return _commandPopupAlwaysOnTop.Value;
            }
            set {
                _commandPopupAlwaysOnTop.Value = value;
            }
        }
        public bool CommandPopupInTaskBar {
            get {
                return _commandPopupInTaskBar.Value;
            }
            set {
                _commandPopupInTaskBar.Value = value;
            }
        }

        public bool AlertOnPasteNewLineChar {
            get {
                return _alertOnPasteNewLineChar.Value;
            }
            set {
                _alertOnPasteNewLineChar.Value = value;
            }
        }


        public Keys IntelliSenseKey {
            get {
                if (_parseKeyRequired)
                    ParseKeys();
                return _intelliSenseKeyCache;
            }
        }
        public Keys CommandPopupKey {
            get {
                if (_parseKeyRequired)
                    ParseKeys();
                return _commandPopupKeyCache;
            }
        }

        public void ResetParseKeyFlag() {
            _parseKeyRequired = true;
        }
        private void ParseKeys() {
            _intelliSenseKeyCache = WinFormsUtil.ParseKey(_intelliSenseKey.Value);
            _commandPopupKeyCache = WinFormsUtil.ParseKey(_commandPopupKey.Value);
            _parseKeyRequired = false;
        }

        public int ShellHistoryLimitCount {
            get {
                return _shellHistoryLimitCount.Value;
            }
        }

        public RenderProfile CreateRenderProfile() {
            //�N���̍������̂��߁A�t�H���g�̍쐬�͒x���]��
            RenderProfile p = new RenderProfile();
            p.FontName = _fontName.Value;
            p.CJKFontName = _cjkFontName.Value;
            p.FontSize = GetFontSizeAsFloat();
            p.UseClearType = _useClearType.Value;
            p.EnableBoldStyle = _enableBoldStyle.Value;
            p.ForceBoldStyle = _forceBoldStyle.Value;
            p.LineSpacing = _lineSpacing.Value;
            p.ESColorSet = (EscapesequenceColorSet)this.EscapeSequenceColorSet.Clone();
            p.DarkenEsColorForBackground = this.DarkenEsColorForBackground;

            p.ForeColor = _textColor.Value;
            p.BackColor = _bgColor.Value;
            p.BackgroundImageFileName = _backgroundImageFileName.Value;
            p.ImageStyle = _imageStyle.Value;

            return p;
        }
        private float GetFontSizeAsFloat() {
            return (float)_fontSize.Value;
        }
    }

    internal class TerminalOptionsSupplier : IPreferenceSupplier, IAdaptable, IPreferenceChangeListener {

        private IPreferenceFolder _originalFolder;
        private TerminalOptions _originalOptions;

        //TerminalOptions���p�ɂɃA�N�Z�X����̂�internal��

        //�^�[�~�i��
        //[ConfigFlagElement(typeof(CaretType), Initial=(int)(CaretType.Blink|CaretType.Box), Max=(int)CaretType.Max)]
        //                                  private CaretType _caretType;

        //[ConfigEnumElement(typeof(DisconnectNotification), InitialAsInt=(int)DisconnectNotification.StatusBar)]
        //                                    private DisconnectNotification _disconnectNotification;
        //[ConfigEnumElement(typeof(LogType), InitialAsInt=(int)LogType.None)]
        //                                    private LogType _defaultLogType;
        //[ConfigStringElement(Initial="")]   private string  _defaultLogDirectory;
        //[ConfigEnumElement(typeof(WarningOption), InitialAsInt=(int)WarningOption.MessageBox)]
        //                                    private WarningOption _warningOption;
        //[ConfigEnumElement(typeof(Keys), InitialAsInt=(int)Keys.None)]
        //                                    private Keys _localBufferScrollModifier;

        //�}�E�X�ƃL�[�{�[�h
        //[ConfigEnumElement(typeof(AltKeyAction), InitialAsInt=(int)AltKeyAction.Menu)]
        //                                    private AltKeyAction _leftAltKey;
        //[ConfigEnumElement(typeof(AltKeyAction), InitialAsInt=(int)AltKeyAction.Menu)]
        //                                    private AltKeyAction _rightAltKey;
        //[ConfigEnumElement(typeof(RightButtonAction), InitialAsInt=(int)RightButtonAction.ContextMenu)]
        //                                    private RightButtonAction _rightButtonAction;


#if false
        //�\��
        [ConfigStringElement(Initial = "Courier New")]
        protected string _fontName;
        [ConfigStringElement(Initial = "�l�r �S�V�b�N")]
        protected string _japaneseFontName;
        [ConfigFloatElement(Initial = 9)]
        protected float _fontSize;


        [ConfigBoolElement(Initial = true)]
        protected bool _useClearType;

        [ConfigColorElement(Initial = LateBindColors.Window)]
        protected Color _bgColor;
        [ConfigColorElement(Initial = LateBindColors.WindowText)]
        protected Color _textColor;
        /* Special Handlings Required */
        protected EscapesequenceColorSet _esColorSet;
        [ConfigStringElement(Initial = "")]
        protected string _backgroundImageFileName;
        [ConfigEnumElement(typeof(ImageStyle), InitialAsInt = (int)ImageStyle.Center)]
        protected ImageStyle _imageStyle;
        [ConfigColorElement(Initial = LateBindColors.Empty)]
        protected Color _caretColor; //Color.Empty�̂Ƃ��͒ʏ�e�L�X�g�𔽓]����̂�

        //�^�[�~�i��
        [ConfigFlagElement(typeof(CaretType), Initial = (int)(CaretType.Blink | CaretType.Box), Max = (int)CaretType.Max)]
        protected CaretType _caretType;

        [ConfigBoolElement(Initial = true)]
        protected bool _closeOnDisconnect;
        [ConfigBoolElement(Initial = false)]
        protected bool _beepOnBellChar;
        [ConfigBoolElement(Initial = false)]
        protected bool _askCloseOnExit;
        [ConfigBoolElement(Initial = false)]
        protected bool _quitAppWithLastPane;
        [ConfigEnumElement(typeof(DisconnectNotification), InitialAsInt = (int)DisconnectNotification.StatusBar)]
        protected DisconnectNotification _disconnectNotification;
        [ConfigIntElement(Initial = 100)]
        protected int _terminalBufferSize;
        [ConfigBoolElement(Initial = false)]
        protected bool _send0x7FByDel;
        [ConfigEnumElement(typeof(LogType), InitialAsInt = (int)LogType.None)]
        protected LogType _defaultLogType;
        [ConfigStringElement(Initial = "")]
        protected string _defaultLogDirectory;
        [ConfigEnumElement(typeof(WarningOption), InitialAsInt = (int)WarningOption.MessageBox)]
        protected WarningOption _warningOption;
        [ConfigBoolElement(Initial = false)]
        protected bool _adjustsTabTitleToWindowTitle;
        [ConfigBoolElement(Initial = false)]
        protected bool _allowsScrollInAppMode;
        [ConfigIntElement(Initial = 0)]
        protected int _keepAliveInterval;
        [ConfigEnumElement(typeof(Keys), InitialAsInt = (int)Keys.None)]
        protected Keys _localBufferScrollModifier;

        //�}�E�X�ƃL�[�{�[�h
        [ConfigEnumElement(typeof(AltKeyAction), InitialAsInt = (int)AltKeyAction.Menu)]
        protected AltKeyAction _leftAltKey;
        [ConfigEnumElement(typeof(AltKeyAction), InitialAsInt = (int)AltKeyAction.Menu)]
        protected AltKeyAction _rightAltKey;
        [ConfigBoolElement(Initial = false)]
        protected bool _autoCopyByLeftButton;
        [ConfigEnumElement(typeof(RightButtonAction), InitialAsInt = (int)RightButtonAction.ContextMenu)]
        protected RightButtonAction _rightButtonAction;
        [ConfigIntElement(Initial = 3)]
        protected int _wheelAmount;
        [ConfigStringElement(Initial = "")]
        protected string _additionalWordElement;
#endif

        public TerminalOptionsSupplier() {
        }

        //IPreferencesupplier
        public void InitializePreference(IPreferenceBuilder builder, IPreferenceFolder folder) {
            _originalFolder = folder;
            _originalOptions = new TerminalOptions(folder);
            _originalOptions.DefineItems(builder);


            //defaultRenderProfile�̃��Z�b�g������
            _originalFolder.AddChangeListener(this);
        }

        public ITerminalEmulatorOptions OriginalOptions {
            get {
                return _originalOptions;
            }
        }

        public object QueryAdapter(IPreferenceFolder folder, Type type) {
            Debug.Assert(folder.Id == _originalFolder.Id);
            if (type == typeof(ITerminalEmulatorOptions))
                return folder == _originalFolder ? _originalOptions : new TerminalOptions(folder).Import(_originalOptions);
            else
                return null;
        }

        public string PreferenceID {
            get {
                return TerminalEmulatorPlugin.PLUGIN_ID;
            }
        }


        public string GetDescription(IPreferenceItem item) {
            return null;
        }

        public void ValidateFolder(IPreferenceFolder folder, IPreferenceValidationResult output) {
        }



        public IAdaptable GetAdapter(Type adapter) {
            return TerminalEmulatorPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }

        #region IPreferenceFolderChangeListener
        //�K��import�����Ƃ����O��Ȃ̂ł�����Ɗ댯
        public void OnPreferenceImport(IPreferenceFolder oldvalues, IPreferenceFolder newvalues) {
            ITerminalEmulatorOptions opt = (ITerminalEmulatorOptions)newvalues.QueryAdapter(typeof(ITerminalEmulatorOptions));

            //DefaultRenderProfile
            GEnv.DefaultRenderProfile = opt.CreateRenderProfile();

            //�K�v��TerminalSession��ApplyTerminalOptions
            ISessionManager sm = TerminalEmulatorPlugin.Instance.GetSessionManager();
            foreach (ISession session in sm.AllSessions) {
                IAbstractTerminalHost ts = (IAbstractTerminalHost)session.GetAdapter(typeof(IAbstractTerminalHost));
                if (ts != null) {
                    TerminalControl tc = ts.TerminalControl;
                    if (tc != null) {
                        tc.ApplyTerminalOptions(opt);
                    }
                }
            }

            //ASCIIWordBreakTable
            ASCIIWordBreakTable table = ASCIIWordBreakTable.Default;
            table.Reset();
            foreach (char ch in opt.AdditionalWordElement)
                table.Set(ch, ASCIIWordBreakTable.LETTER);

            //�L�[�o�C���h�n�����Z�b�g
            TerminalEmulatorPlugin.Instance.CustomKeySettings.Reset(opt);

            //KeepAlive�̃��t���b�V��
            TerminalEmulatorPlugin.Instance.KeepAlive.Refresh(opt.KeepAliveInterval);

            _originalOptions.ResetParseKeyFlag();
        }

        public void OnChange(IPreferenceItem item) {
        }
        #endregion
    }

    /// <summary>
    /// <ja>�y�C���̈ʒu���w�肵�܂��B</ja>
    /// <en>Set position of pane.</en>
    /// </summary>
    public enum PanePosition {
        /// <summary>
        /// <ja>�㉺�����̂Ƃ��̏�A���E�����̂Ƃ��̍��̈ʒu�ł��B</ja>
        /// <en>upper side when vertical division, left side transverse division.</en>
        /// </summary>
        First,
        /// <summary>
        /// <ja>�㉺�����̂Ƃ��̉��A���E�����̂Ƃ��̉E�̈ʒu�ł��B</ja>
        /// <en>lower side when vertical division, right side transverse division.</en>
        /// </summary>
        Second
    }

    //�������ȕ����������Ƃ��ǂ����邩
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum WarningOption {
        [EnumValue(Description = "Enum.WarningOption.Ignore")]
        Ignore,
        [EnumValue(Description = "Enum.WarningOption.StatusBar")]
        StatusBar,
        [EnumValue(Description = "Enum.WarningOption.MessageBox")]
        MessageBox
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum AltKeyAction {
        [EnumValue(Description = "Enum.AltKeyAction.Menu")]
        Menu,
        [EnumValue(Description = "Enum.AltKeyAction.ESC")]
        ESC,
        [EnumValue(Description = "Enum.AltKeyAction.Meta")]
        Meta
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum MouseButtonAction {
        [EnumValue(Description = "Enum.MouseButtonAction.None")]
        None,
        [EnumValue(Description = "Enum.MouseButtonAction.ContextMenu")]
        ContextMenu,
        [EnumValue(Description = "Enum.MouseButtonAction.Paste")]
        Paste
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum KeyboardStyle {
        [EnumValue(Description = "Enum.KeyboardStyle.None")]
        None,
        [EnumValue(Description = "Enum.KeyboardStyle.Default")]
        Default,
        [EnumValue(Description = "Enum.KeyboardStyle.Japanese")]
        Japanese
    }

    /// <summary>
    /// <ja>
    /// �I�v�V�������s���ȂƂ��ɔ��������O�ł��B
    /// </ja>
    /// <en>
    /// Exception generated when option is illegal.
    /// </en>
    /// </summary>
    public class InvalidOptionException : Exception {
        /// <summary>
        /// <ja>
        /// �I�v�V�������s���ȂƂ��ɔ��������O���쐬���܂��B
        /// </ja>
        /// <en>
        /// Generate Exception when option is illegal
        /// </en>
        /// </summary>
        /// <param name="msg"><ja>��O�̃��b�Z�[�W�ł��B</ja>
        /// <en>Message of exception.</en>
        /// </param>
        public InvalidOptionException(string msg)
            : base(msg) {
        }
    }
}
