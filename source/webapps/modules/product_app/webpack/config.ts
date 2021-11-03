// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import { sealedMerge } from 'app-settings-loader/sealedMerge';
import fs from 'fs-extra';
import path from 'path';

// Build mode.
export const BUILD_ENV = process.env.NODE_ENV || 'development';
export const IS_PROD = BUILD_ENV === 'production';

// Paths.
export const PRJ_DIR = path.dirname(__dirname);
export const DST_DIR = path.join(PRJ_DIR, 'dist');
export const SRC_DIR = path.join(PRJ_DIR, 'src');
export const TMP_DIR = path.join(PRJ_DIR, '.tmp');
export const PUB_DIR = path.join(PRJ_DIR, 'public');

// Settings.
const loadSettingsFile = (file: string): Record<string, unknown> => (fs.existsSync(file) ? fs.readJsonSync(file) : {});
const settings = sealedMerge(
  loadSettingsFile(path.join(PRJ_DIR, 'settings.json')),
  loadSettingsFile(path.join(PRJ_DIR, `settings.${BUILD_ENV}.json`))
) as Record<string, unknown>;

export const APP_TITLE = settings.appTitle as string;
export const BASE_URL = settings.baseUrl as string;
export const DEBUG_MODE = settings.debugMode as boolean;
export const MAX_SIZE_DATA_URL = settings.maxSizeDataUrl as number;
export const SERVER_HOST = settings.serverHost as string | undefined;
export const SERVER_PORT = settings.serverPort as number | undefined;
export const TEST_MODE = settings.testMode as boolean;
export const PORT = settings.port as number;
export const FOLDER_PATH = settings.folderPath as string;
