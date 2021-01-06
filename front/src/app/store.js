import { configureStore } from '@reduxjs/toolkit';
import counterReducer from '../features/counter/counterSlice';
import sslForm from '../features/ssl-form/sslFormSlice';

export default configureStore({
  reducer: {
    counter: counterReducer,
    sslForm
  },
});
