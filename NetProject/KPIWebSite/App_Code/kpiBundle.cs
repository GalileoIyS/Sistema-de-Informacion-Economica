using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using BundleTransformer.Core.Transformers;


/// <summary>
/// Summary description for kpiBundle
/// </summary>
public class kpiBundle
{
	public kpiBundle()
	{
		//
		// TODO: Add constructor logic here
        // 
	}
    public static void RegisterBundles(BundleCollection bundles)
      {
         bundles.UseCdn = true;

         var nullBuilder = new NullBuilder();
         var styleTransformer = new StyleTransformer();
         var scriptTransformer = new ScriptTransformer();
         var nullOrderer = new NullOrderer();

         // Replace a default bundle resolver in order to the debugging HTTP-handler
         // can use transformations of the corresponding bundle
         BundleResolver.Current = new CustomBundleResolver();

         Bundle generalScriptsBundle = new Bundle("~/js/kpigeneralV100");
         generalScriptsBundle.Include(
                                "~/scripts/generales/menusecundario.js",
                                "~/scripts/generales/proxy.js");
         generalScriptsBundle.Transforms.Add(scriptTransformer);
         generalScriptsBundle.Orderer = nullOrderer;
         bundles.Add(generalScriptsBundle);

         Bundle classesTemplatesBundle = new Bundle("~/js/kpiclassesV100");
         classesTemplatesBundle.Include(
                                 "~/scripts/custom/classes/kpiboard.avisos.js",
                                 "~/scripts/custom/classes/kpiboard.comments.js",
                                 "~/scripts/custom/classes/kpiboard.daily.js",
                                 "~/scripts/custom/classes/kpiboard.dashboard.js",
                                 "~/scripts/custom/classes/kpiboard.dataset.js",
                                 "~/scripts/custom/classes/kpiboard.dimension.js",
                                 "~/scripts/custom/classes/kpiboard.dimensionvalues.js",
                                 "~/scripts/custom/classes/kpiboard.expressions.js",
                                 "~/scripts/custom/classes/kpiboard.filter.js",
                                 "~/scripts/custom/classes/kpiboard.formula.js",
                                 "~/scripts/custom/classes/kpiboard.friendship.js",
                                 "~/scripts/custom/classes/kpiboard.function.js",
                                 "~/scripts/custom/classes/kpiboard.importAttribute.js",
                                 "~/scripts/custom/classes/kpiboard.importColumn.js",
                                 "~/scripts/custom/classes/kpiboard.importdata.js",
                                 "~/scripts/custom/classes/kpiboard.indicator.js",
                                 "~/scripts/custom/classes/kpiboard.revision.js",
                                 "~/scripts/custom/classes/kpiboard.subcategory.js",
                                 "~/scripts/custom/classes/kpiboard.user.js",
                                 "~/scripts/custom/classes/kpiboard.widget.js");
         classesTemplatesBundle.Builder = nullBuilder;
         classesTemplatesBundle.Transforms.Add(scriptTransformer);
         classesTemplatesBundle.Orderer = nullOrderer;
         bundles.Add(classesTemplatesBundle);

         Bundle objectsTemplatesBundle = new Bundle("~/js/kpiobjectsV100");
         objectsTemplatesBundle.Include(
                                 "~/scripts/custom/objects/dropkeys.attributes.js",
                                 "~/scripts/custom/objects/dropkeys.comment.js",
                                 "~/scripts/custom/objects/dropkeys.dashboard.js",
                                 "~/scripts/custom/objects/dropkeys.datasets.js",
                                 "~/scripts/custom/objects/dropkeys.editfilter.js",
                                 "~/scripts/custom/objects/dropkeys.formulaInput.js",
                                 "~/scripts/custom/objects/dropkeys.formulas.js",
                                 "~/scripts/custom/objects/dropkeys.friendship.js",
                                 "~/scripts/custom/objects/dropkeys.imports.js",
                                 "~/scripts/custom/objects/dropkeys.indicator.js",
                                 "~/scripts/custom/objects/dropkeys.listfilters.js",
                                 "~/scripts/custom/objects/dropkeys.newfilter.js",
                                 "~/scripts/custom/objects/dropkeys.reply.js",
                                 "~/scripts/custom/objects/dropkeys.user.js",
                                 "~/scripts/custom/objects/dropkeys.widget.js");
         objectsTemplatesBundle.Builder = nullBuilder;
         objectsTemplatesBundle.Transforms.Add(scriptTransformer);
         objectsTemplatesBundle.Orderer = nullOrderer;
         bundles.Add(objectsTemplatesBundle);

         Bundle customTemplatesBundle = new Bundle("~/js/customSmartAdminV100");
         customTemplatesBundle.Include(
                                 "~/scripts/custom/smartadmin/smartadmin.jarvismenuitem.js");
         customTemplatesBundle.Builder = nullBuilder;
         customTemplatesBundle.Transforms.Add(scriptTransformer);
         customTemplatesBundle.Orderer = nullOrderer;
         bundles.Add(customTemplatesBundle);

         Bundle controlsTemplatesBundle = new Bundle("~/js/kpicontrolsV100");
         controlsTemplatesBundle.Include(
                                 "~/scripts/custom/controls/dropkeys.kpicontrols.js",
                                 "~/scripts/custom/controls/dropkeys.kpimessage.js",
                                 "~/scripts/custom/controls/dropkeys.shortlist.js");
         controlsTemplatesBundle.Builder = nullBuilder;
         controlsTemplatesBundle.Transforms.Add(scriptTransformer);
         controlsTemplatesBundle.Orderer = nullOrderer;
         bundles.Add(controlsTemplatesBundle);

      }
}