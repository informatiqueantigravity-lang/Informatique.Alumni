import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44338/',
  redirectUri: baseUrl,
  clientId: 'Alumni_App',
  responseType: 'code',
  scope: 'offline_access Alumni',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Alumni',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44338',
      rootNamespace: 'Informatique.Alumni',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
