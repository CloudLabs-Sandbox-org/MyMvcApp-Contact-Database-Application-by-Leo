using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class UserController : Controller
{
    public static System.Collections.Generic.List<User> userlist = new System.Collections.Generic.List<User>();

    public ActionResult Index(string searchName)
    {
        // Filtra la lista de usuarios si se proporciona un nombre de búsqueda
        var filteredUsers = string.IsNullOrEmpty(searchName)
            ? userlist
            : userlist.Where(u => u.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

        // Devuelve la vista con la lista filtrada
        return View(filteredUsers);
    }

    // GET: User/Details/5
    public ActionResult Details(int id)
    {
        // Busca el usuario en la lista por ID
        var user = userlist.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            // Si no se encuentra el usuario, devuelve un resultado de "No encontrado"
            return NotFound();
        }

        // Devuelve la vista con los detalles del usuario
        return View(user);
    }
    public ActionResult Create()
    {
        // Devuelve la vista para crear un nuevo usuario
        return View();
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(User user)
    {
        // Asigna el ID si está en cero o si deseas incrementarlo siempre
        user.Id = userlist.Any() ? userlist.Max(u => u.Id) + 1 : 1;
        // Verifica si el modelo es válido
        if (ModelState.IsValid)
        {
            // Agrega el nuevo usuario a la lista
            userlist.Add(user);

            // Redirige al índice para mostrar la lista actualizada
            return RedirectToAction("Index");
        }

        // Si el modelo no es válido, devuelve la vista con los errores
        return View(user);
    }
    // GET: User/Edit/5
    public ActionResult Edit(int id)
    {
        // Busca el usuario en la lista por ID
        var user = userlist.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            // Si no se encuentra el usuario, devuelve un resultado de "No encontrado"
            return NotFound();
        }

        // Devuelve la vista con el usuario a editar
        return View(user);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, User updatedUser)
    {
        // Busca el usuario en la lista por ID
        var user = userlist.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            // Si no se encuentra el usuario, devuelve un resultado de "No encontrado"
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Actualiza los datos del usuario
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            // Redirige al índice para mostrar la lista actualizada
            return RedirectToAction("Index");
        }

        // Si el modelo no es válido, devuelve la vista con los errores
        return View(updatedUser);
    }

    // GET: User/Delete/5
    public ActionResult Delete(int id)
    {
        // Busca el usuario en la lista por ID
        var user = userlist.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            // Si no se encuentra el usuario, devuelve un resultado de "No encontrado"
            return NotFound();
        }

        // Devuelve la vista con el usuario a eliminar
        return View(user);
    }

    // POST: User/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        // Busca el usuario en la lista por ID
        var user = userlist.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            // Si no se encuentra el usuario, devuelve un resultado de "No encontrado"
            return NotFound();
        }

        // Elimina el usuario de la lista
        userlist.Remove(user);

        // Redirige al índice para mostrar la lista actualizada
        return RedirectToAction("Index");
    }
}
