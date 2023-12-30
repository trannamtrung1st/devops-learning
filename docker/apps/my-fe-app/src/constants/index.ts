export interface IDynamicSettings {
    webApiUrl: string;
}

export const dynamicSettings: IDynamicSettings = {
    webApiUrl: process.env.NEXT_PUBLIC_API || ''
};