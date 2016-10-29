using System.Configuration;
using System.Web.Mvc;
using System.Web.Configuration;
using System;
using my.ns.entities.dto.administration;

namespace my.ns.client.Areas.Admin.Controllers

{
    public class SettingsController : BaseController
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /*

Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
string oldValue = config.AppSettings.Settings["SomeKey"].Value;
config.AppSettings.Settings["SomeKey"].Value = "NewValue";
config.Save(ConfigurationSaveMode.Modified);
*/
        // GET: Manage/Settings
        public ActionResult Index()
        {
            log.Trace("ApplicationSettings Index view opened");
            return View(ConfigurationManager.AppSettings);
        }

        // GET: Manage/Settings/Create
        public ActionResult Create()
        {
            log.Trace("ApplicationSettings Create view opened");
            return View();
        }

        // POST: Manage/Settings/Create
        [HttpPost]
        public ActionResult Create(SettingsModel _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                    var _value = _model.value;
                    if (config.AppSettings.Settings[_model.key] != null)
                    {
                        ModelState.AddModelError("key", "Object already exists, edit value instead");
                    }
                    else {
                        config.AppSettings.Settings.Add(_model.key, _model.value);
                        config.Save(ConfigurationSaveMode.Modified);
                        log.Info("Created new ApplicationSetting: {" + _model.key + ": " + _model.value + "}");
                        return RedirectToAction("Index");
                    }
                }

                return View(_model);
            }
            catch (Exception ex)
            {
                log.Error("Error while editing setting", ex);
                return View(_model);
            }
        }

        // POST: Manage/Settings/Edit/5
        [HttpPost]
        public ActionResult Edit(SettingsModel _model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                    string oldValue = config.AppSettings.Settings[_model.key].Value;
                    config.AppSettings.Settings[_model.key].Value = _model.value;
                    config.Save(ConfigurationSaveMode.Modified);
                    log.Info("Edited ApplicationSetting: {" + _model.key + ": " + _model.value + "}");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Please correct error");
                    log.Error("Error while editing setting");
                    return View(_model);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Please correct error");
                log.Error("Error while editing setting", ex);
                return View(_model);
            }
        }

        // GET: Manage/Settings/Delete/5
        public ActionResult Delete(string id)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
            config.AppSettings.Settings.Remove(id);
            config.Save(ConfigurationSaveMode.Modified);
            log.Info("Created new ApplicationSetting: "+id);
            return RedirectToAction("Index");
        }
    }
}
