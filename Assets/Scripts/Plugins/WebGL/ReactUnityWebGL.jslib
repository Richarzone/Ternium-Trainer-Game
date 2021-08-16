mergeInto(LibraryManager.library, {
  CreateJuego: function (message) {
    ReactUnityWebGL.CreateJuego(Pointer_stringify(message));
  }
});
