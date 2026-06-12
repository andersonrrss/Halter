import { 
  ApplicationConfig, 
  provideBrowserGlobalErrorListeners,
  inject
} from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideTranslateService } from '@ngx-translate/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    provideTranslateService({
      lang: 'pt-BR',
      fallbackLang: 'en',
      loader: provideTranslateHttpLoader({
        prefix: "/i18n/",
        suffix: ".json"
      })
    })
  ]
};
