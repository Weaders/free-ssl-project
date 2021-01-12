import { configureStore } from '@reduxjs/toolkit';
import mainDataReducer from './store-data/main'
import sslFormReducer  from './store-data/ssl-form'

export default configureStore({
  reducer: {
    sslForm: sslFormReducer,
    main: mainDataReducer
  }
});